using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Logging;
using Nop.Core.Infrastructure;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Plugins;
using Nop.Plugin.InstantSearch.Theme;
using System.Text.RegularExpressions;
using System.Xml.Linq;


#nullable enable
namespace Nop.Plugin.InstantSearch
{
  public class InstallHelper : IInstallHelper
  {
    private const 
    #nullable disable
    string SupportedWidgetZonesKeyFormat = "{0}-{1}";
    private static readonly Dictionary<string, Dictionary<string, IEnumerable<string>>> SupportedWidgetZonesByPluginAndThemePerFiles = new Dictionary<string, Dictionary<string, IEnumerable<string>>>();
    private static readonly Dictionary<string, IEnumerable<string>> SupportedWidgetZonesByPluginAndTheme = new Dictionary<string, IEnumerable<string>>();
    private static readonly object supportedWidgetZonesByPluginAndThemePerFilesLockObject = new object();
    private static readonly object supportedWidgetZonesByPluginAndThemeLockObject = new object();
    private ISettingService _settingService;

    protected IEngine Engine { get; set; }

    protected INopFileProvider NopFileProvider { get; set; }

    protected IPluginService PluginService { get; set; }

    private ISettingService SettingService
    {
      get
      {
        if (this._settingService == null)
          this._settingService = this.Engine.Resolve<ISettingService>((IServiceScope) null);
        return this._settingService;
      }
    }

    public InstallHelper(
      IEngine engine,
      INopFileProvider nopFileProvider,
      IPluginService pluginService)
    {
      this.Engine = engine;
      this.NopFileProvider = nopFileProvider;
      this.PluginService = pluginService;
    }

    public async Task InstallLocaleResourcesAsync(
      string pluginFolderName,
      string fileNameWithoutCultureAndExtension = "Resources",
      bool updateExistingResources = true)
    {
      string resourcesDirectoryPath = this.NopFileProvider.MapPath(string.Format("~/Plugins/{0}/Resources/", (object) pluginFolderName));
      ILanguageService languageService;
      string searchPattern;
      IList<Language> languages;
      if (!Directory.Exists(resourcesDirectoryPath))
      {
        resourcesDirectoryPath = (string) null;
        languageService = (ILanguageService) null;
        searchPattern = (string) null;
        languages = (IList<Language>) null;
      }
      else
      {
        languageService = this.Engine.Resolve<ILanguageService>((IServiceScope) null);
        searchPattern = string.Format("{0}.*.xml", (object) fileNameWithoutCultureAndExtension);
        languages = await languageService.GetAllLanguagesAsync(false, 0);
        foreach (Language activeLanguage in (IEnumerable<Language>) languages)
        {
          string str = this.NopFileProvider.MapPath(string.Format("~/Plugins/{0}/Resources/{1}.en-us.xml", (object) pluginFolderName, (object) fileNameWithoutCultureAndExtension));
          if (File.Exists(str))
          {
            if (activeLanguage.LanguageCulture.ToLower() != "en-us")
              await this.InstallLanguageResourcesFromXmlAsync(languageService, str, activeLanguage, false);
            else
              await this.InstallLanguageResourcesFromXmlAsync(languageService, str, activeLanguage, updateExistingResources);
          }
        }
        foreach (string str in Directory.EnumerateFiles(resourcesDirectoryPath, searchPattern).Where<string>((Func<string, bool>) (f => !f.EndsWith(".en-us.xml"))))
        {
          Match match = new Regex("\\.([a-zA-Z-]{5})(?:\\.xml)$").Match(str);
          if (match.Success && match.Groups.Count > 0)
          {
            Group languageCultureGroup = match.Groups[1];
            if (!string.IsNullOrWhiteSpace(languageCultureGroup.Value))
            {
              Language activeLanguage = languages.FirstOrDefault<Language>((Func<Language, bool>) (x => x.LanguageCulture.ToLower() == languageCultureGroup.Value.ToLower()));
              if (activeLanguage != null)
                await this.InstallLanguageResourcesFromXmlAsync(languageService, str, activeLanguage, updateExistingResources);
            }
          }
        }
        resourcesDirectoryPath = (string) null;
        languageService = (ILanguageService) null;
        searchPattern = (string) null;
        languages = (IList<Language>) null;
      }
    }

    public async Task InstallDefaultPluginSettingsAsync(
      string pluginFolderName,
      bool overrideExistingSettings = true)
    {
      string filePath = this.NopFileProvider.MapPath(string.Format("~/Plugins/{0}/Settings.xml", (object) pluginFolderName));
      Stream settingsFileStream;
      if (!File.Exists(filePath))
      {
        filePath = (string) null;
        settingsFileStream = (Stream) null;
      }
      else
      {
        settingsFileStream = (Stream) null;
        try
        {
          try
          {
            settingsFileStream = (Stream) new FileStream(filePath, FileMode.Open, FileAccess.Read);
            XElement xelement1 = XDocument.Load(settingsFileStream).Element((XName) "Settings");
            if (xelement1 == null)
              throw new InvalidOperationException("Settings element not found in file {0}");
            foreach (XElement element in xelement1.Elements((XName) "Setting"))
            {
              XAttribute xattribute = element.Attribute((XName) "Name");
              XElement xelement2 = element.Element((XName) "Value");
              if (xattribute != null && xelement2 != null)
              {
                string settingName = xattribute.Value;
                string settingValue = xelement2.Value;
                if (!string.IsNullOrWhiteSpace(settingName) && settingValue != null)
                {
                  if (!overrideExistingSettings)
                  {
                    if (await this.SettingService.GetSettingByKeyAsync<string>(settingName, (string) null, 0, false) == null)
                      await this.SettingService.SetSettingAsync<string>(settingName, settingValue, 0, true);
                  }
                  else
                    await this.SettingService.SetSettingAsync<string>(settingName, settingValue, 0, true);
                }
                settingName = (string) null;
                settingValue = (string) null;
              }
            }
          }
          catch (Exception ex)
          {
            Log log = await EngineContext.Current.Resolve<ILogger>((IServiceScope) null).InsertLogAsync((LogLevel) 40, string.Format("Error installing settings from file {0}", (object) filePath), ex.ToString(), (Customer) null);
          }
          filePath = (string) null;
          settingsFileStream = (Stream) null;
        }
        finally
        {
          settingsFileStream?.Close();
        }
      }
    }

    private async Task InstallLanguageResourcesFromXmlAsync(
      ILanguageService languageService,
      string filePath,
      Language activeLanguage,
      bool updateExistingResources)
    {
      try
      {
        using (StreamReader streamReader = new StreamReader(filePath))
          await EngineContext.Current.Resolve<ILocalizationService>((IServiceScope) null).ImportResourcesFromXmlAsync(activeLanguage, streamReader, updateExistingResources);
      }
      catch (Exception ex)
      {
        Log log = await EngineContext.Current.Resolve<ILogger>((IServiceScope) null).InsertLogAsync((LogLevel) 40, string.Format("Error installing resources from file {0}", (object) filePath), ex.ToString(), (Customer) null);
      }
    }

    public async Task<IEnumerable<string>> GetSupportedWidgetZonesAsync(string pluginFolderName)
    {
      string desktopThemeAsync = await ThemeHelper.GetCurrentDesktopThemeAsync();
      return this.GetSupportedWidgetZonesForTheme(pluginFolderName, desktopThemeAsync);
    }

    public async Task<IEnumerable<string>> GetSupportedWidgetZonesAsync(
      string pluginFolderName,
      int storeId)
    {
      string desktopThemeAsync = await ThemeHelper.GetCurrentAdminDesktopThemeAsync(storeId);
      return this.GetSupportedWidgetZonesForTheme(pluginFolderName, desktopThemeAsync);
    }

    private IEnumerable<string> GetSupportedWidgetZonesForTheme(
      string pluginFolderName,
      string themeName)
    {
      string key = string.Format("{0}-{1}", (object) pluginFolderName, (object) themeName);
      if (InstallHelper.SupportedWidgetZonesByPluginAndTheme.ContainsKey(key))
        return InstallHelper.SupportedWidgetZonesByPluginAndTheme[key];
      List<string> widgetZonesForTheme = new List<string>();
      foreach (KeyValuePair<string, IEnumerable<string>> zoneForPluginByFile in this.GetSupportedWidgetZoneForPluginByFiles(pluginFolderName, themeName))
        widgetZonesForTheme.AddRange(zoneForPluginByFile.Value);
      lock (InstallHelper.supportedWidgetZonesByPluginAndThemeLockObject)
      {
        if (!InstallHelper.SupportedWidgetZonesByPluginAndTheme.ContainsKey(key))
          InstallHelper.SupportedWidgetZonesByPluginAndTheme.Add(key, (IEnumerable<string>) widgetZonesForTheme);
      }
      return (IEnumerable<string>) widgetZonesForTheme;
    }

    public async Task<IEnumerable<string>> GetSupportedWidgetZonesAsync(
      string pluginFolderName,
      string fileName)
    {
      string desktopThemeAsync = await ThemeHelper.GetCurrentDesktopThemeAsync();
      Dictionary<string, IEnumerable<string>> forPluginByFiles = this.GetSupportedWidgetZoneForPluginByFiles(pluginFolderName, desktopThemeAsync);
      return !forPluginByFiles.ContainsKey(fileName) ? (IEnumerable<string>) new List<string>() : forPluginByFiles[fileName];
    }

    public async Task<IEnumerable<string>> GetSupportedWidgetZonesAsync(
      string pluginFolderName,
      string fileName,
      int storeId)
    {
      string desktopThemeAsync = await ThemeHelper.GetCurrentAdminDesktopThemeAsync(storeId);
      Dictionary<string, IEnumerable<string>> forPluginByFiles = this.GetSupportedWidgetZoneForPluginByFiles(pluginFolderName, desktopThemeAsync);
      return !forPluginByFiles.ContainsKey(fileName) ? (IEnumerable<string>) new List<string>() : forPluginByFiles[fileName];
    }

    private Dictionary<string, IEnumerable<string>> GetSupportedWidgetZoneForPluginByFiles(
      string pluginFolderName,
      string themeName)
    {
      string str = string.Format("{0}-{1}", (object) pluginFolderName, (object) themeName);
      if (!InstallHelper.SupportedWidgetZonesByPluginAndThemePerFiles.ContainsKey(str))
      {
        lock (InstallHelper.supportedWidgetZonesByPluginAndThemePerFilesLockObject)
        {
          if (!InstallHelper.SupportedWidgetZonesByPluginAndThemePerFiles.ContainsKey(str))
            this.PrepareWidgetZones(pluginFolderName, themeName, str);
        }
      }
      return InstallHelper.SupportedWidgetZonesByPluginAndThemePerFiles[str];
    }

    private void PrepareWidgetZones(
      string pluginFolderName,
      string themeName,
      string pluginThemeKey)
    {
      Dictionary<string, IEnumerable<string>> second = this.GetGeneralSupportedWidgetZones(pluginFolderName);
      Dictionary<string, IEnumerable<string>> supportedWidgetZones1 = this.GetThemeSupportedWidgetZones(pluginFolderName, themeName);
      Dictionary<string, IEnumerable<string>> supportedWidgetZones2 = this.GetThemeAdditionalSupportedWidgetZones(pluginFolderName, themeName);
      if (supportedWidgetZones1 != null && supportedWidgetZones1.Values.Any<IEnumerable<string>>())
      {
        Dictionary<string, IEnumerable<string>> dictionary = new Dictionary<string, IEnumerable<string>>((IDictionary<string, IEnumerable<string>>) second);
        foreach (KeyValuePair<string, IEnumerable<string>> keyValuePair in supportedWidgetZones1)
          dictionary[keyValuePair.Key] = keyValuePair.Value;
        second = dictionary;
      }
      Dictionary<string, IEnumerable<string>> dictionary1 = supportedWidgetZones2.Union<KeyValuePair<string, IEnumerable<string>>>((IEnumerable<KeyValuePair<string, IEnumerable<string>>>) second).ToList<KeyValuePair<string, IEnumerable<string>>>().ToDictionary<KeyValuePair<string, IEnumerable<string>>, string, IEnumerable<string>>((Func<KeyValuePair<string, IEnumerable<string>>, string>) (x => x.Key), (Func<KeyValuePair<string, IEnumerable<string>>, IEnumerable<string>>) (y => y.Value));
      if (InstallHelper.SupportedWidgetZonesByPluginAndThemePerFiles.ContainsKey(pluginThemeKey))
        return;
      InstallHelper.SupportedWidgetZonesByPluginAndThemePerFiles.Add(pluginThemeKey, dictionary1);
    }

    private Dictionary<string, IEnumerable<string>> GetGeneralSupportedWidgetZones(
      string pluginFolderName)
    {
      return this.PrepareWidgetZonesForFiles(string.Format("~/Plugins/{0}", (object) pluginFolderName), "SupportedWidgetZones");
    }

    private Dictionary<string, IEnumerable<string>> GetThemeAdditionalSupportedWidgetZones(
      string pluginFolderName,
      string themeName)
    {
      return this.PrepareWidgetZonesForFiles(string.Format("~/Plugins/{0}/Theme/{1}", (object) pluginFolderName, (object) themeName), "AdditionalSupportedWidgetZones");
    }

    private Dictionary<string, IEnumerable<string>> GetThemeSupportedWidgetZones(
      string pluginFolderName,
      string themeName)
    {
      return this.PrepareWidgetZonesForFiles(string.Format("~/Plugins/{0}/Theme/{1}", (object) pluginFolderName, (object) themeName), "SupportedWidgetZones");
    }

    private Dictionary<string, IEnumerable<string>> PrepareWidgetZonesForFiles(
      string pathToSearch,
      string filesWhichStartsWith)
    {
      Dictionary<string, IEnumerable<string>> dictionary = new Dictionary<string, IEnumerable<string>>();
      string path = this.NopFileProvider.MapPath(pathToSearch);
      if (!Directory.Exists(path))
        return dictionary;
      foreach (string str in ((IEnumerable<string>) Directory.GetFiles(path, "*.xml")).Where<string>((Func<string, bool>) (file => Path.GetFileName(file).StartsWith(filesWhichStartsWith))))
      {
        IEnumerable<string> strings = InstallHelper.ReadWidgetZones(str);
        dictionary.Add(Path.GetFileNameWithoutExtension(str), strings);
      }
      return dictionary;
    }

    private static IEnumerable<string> ReadWidgetZones(string filePath)
    {
      List<string> stringList = new List<string>();
      if (!File.Exists(filePath))
        return (IEnumerable<string>) stringList;
      XElement xelement = XDocument.Load(filePath).Element((XName) "SupportedWidgetZones");
      if (xelement == null)
        throw new InvalidOperationException(string.Format("SupportedWidgetZones element not found in file {0}", (object) filePath));
      foreach (XElement element in xelement.Elements((XName) "WidgetZone"))
      {
        string str = element.Value;
        if (!string.IsNullOrWhiteSpace(str))
          stringList.Add(str);
      }
      return (IEnumerable<string>) stringList;
    }
  }
}
