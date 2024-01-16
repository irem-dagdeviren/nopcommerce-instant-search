using Nop.Services.Configuration;
using System.Globalization;

#nullable enable
namespace Nop.Plugin.InstantSearch.Theme
{
  public class ThemeService : IThemeService
  {
    private 
    #nullable disable
    ISettingService _settingService;

    public ThemeService(ISettingService settingService) => this._settingService = settingService;

    public async Task<bool> CheckActiveThemeAsync(string themeName, string themeVariantsSettingsKey)
    {
      if (string.IsNullOrEmpty(themeVariantsSettingsKey) && string.IsNullOrEmpty(themeName))
        return true;
      string currentDesktopTheme = await ThemeHelper.GetCurrentDesktopThemeAsync();
      if (!string.IsNullOrEmpty(themeName) && string.Compare(themeName, currentDesktopTheme, true, CultureInfo.InvariantCulture) == 0)
        return true;
      if (!string.IsNullOrEmpty(themeVariantsSettingsKey))
      {
        string settingByKeyAsync = await this._settingService.GetSettingByKeyAsync<string>(themeVariantsSettingsKey, (string) null, 0, false);
        if (!string.IsNullOrEmpty(settingByKeyAsync))
        {
          string str = settingByKeyAsync;
          char[] separator = new char[1]{ ';' };
          foreach (string strA in str.Split(separator, StringSplitOptions.RemoveEmptyEntries))
          {
            if (string.Compare(strA, currentDesktopTheme, true, CultureInfo.InvariantCulture) == 0)
              return true;
          }
        }
      }
      return false;
    }
  }
}
