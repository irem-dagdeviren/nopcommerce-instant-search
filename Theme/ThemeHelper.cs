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
        string _defaultThemeName = "DefaultClean";

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
            return path != null && Directory.Exists(path) ? await ThemeHelper.ThemeContext.GetWorkingThemeNameAsync() : ThemeHelper._defaultThemeName;
        }
    }
}
