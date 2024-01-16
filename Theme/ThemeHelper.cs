using Microsoft.Extensions.DependencyInjection;
using Nop.Core;
using Nop.Core.Infrastructure;
using Nop.Services.Configuration;
using Nop.Web.Framework.Themes;

#nullable enable
namespace Nop.Plugin.InstantSearch.Theme
{
  public class ThemeHelper
  {
    private static readonly 
    #nullable disable
    string DefaultThemeName = "DefaultClean";

    public static async Task<string> GetCurrentDesktopThemeAsync() => await ThemeHelper.ThemeContext.GetWorkingThemeNameAsync();

    public static async Task<string> GetCurrentAdminDesktopThemeAsync(int storeId) => await EngineContext.Current.Resolve<ISettingService>((IServiceScope) null).GetSettingByKeyAsync<string>("storeinformationsettings.defaultstoretheme", (string) null, storeId, true);

    internal static IThemeContext ThemeContext => EngineContext.Current.Resolve<IThemeContext>((IServiceScope) null);

    public static async Task<string> GetPluginThemeAsync(string pluginFolderName) => await ThemeHelper.GetPluginThemeInternalAsync(pluginFolderName);

    private static async Task<string> GetPluginThemeInternalAsync(string pluginFolderName)
    {
      INopFileProvider inopFileProvider = CommonHelper.DefaultFileProvider;
      string str = pluginFolderName;
      string path = inopFileProvider.MapPath("~/Plugins/" + str + "/Theme/" + await ThemeHelper.ThemeContext.GetWorkingThemeNameAsync());
      inopFileProvider = (INopFileProvider) null;
      str = (string) null;
      return path != null && Directory.Exists(path) ? await ThemeHelper.ThemeContext.GetWorkingThemeNameAsync() : ThemeHelper.DefaultThemeName;
    }

    public static async Task<string> GetPluginViewPathAsync(
      string pluginFolderName,
      string viewPath)
    {
      return await ThemeHelper.GetPluginViewPathInternalAsync(pluginFolderName, viewPath);
    }

    private static async Task<string> GetPluginViewPathInternalAsync(
      string pluginFolderName,
      string view)
    {
      string str1 = "Theme/" + await ThemeHelper.ThemeContext.GetWorkingThemeNameAsync();
      string str2 = string.Format(view, (object) str1);
      string str3 = string.Format(view, (object) string.Empty);
      string path = CommonHelper.DefaultFileProvider.MapPath("~/Plugins/" + pluginFolderName + "/" + str1);
      return path == null || !Directory.Exists(path) ? str3 : (!File.Exists(CommonHelper.DefaultFileProvider.MapPath(str2)) ? str3 : str2);
    }
  }
}
