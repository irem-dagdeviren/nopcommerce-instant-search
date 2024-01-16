using Nop.Core.Domain.Cms;
using Nop.Core.Infrastructure;
using Nop.Services.Configuration;
using Nop.Plugin.InstantSearch.Components;

namespace Nop.Plugin.InstantSearch.Plugin
{
    public class InstantSearchPlugin : BaseAdminWidgetPlugin7Spikes
    {
        private readonly WidgetSettings _widgetSettings;
        private readonly ISettingService _settingService;
        private static readonly List<MenuItem7Spikes> MenuItems;

        private static bool IsTrialVersion => false;

        public InstantSearchPlugin(WidgetSettings widgetSettings, ISettingService settingService)
          : base(MenuItems, "Nop.Plugin.InstantSearch.Admin.Menu.MenuName", "InstantSearch", IsTrialVersion, "http://www.nop-templates.com/instant-search-plugin-for-nopcommerce")
        {
            this._widgetSettings = widgetSettings;
            this._settingService = settingService;
        }

        public override string GetConfigurationPageUrl() => this.StoreLocation + "Admin/InstantSearchAdmin/Settings";

        protected override async Task InstallAdditionalSettingsAsync()
        {
            await EngineContext.Current.Resolve<ISettingService>(null).SetSettingAsync("catalogsettings.productsearchautocompleteenabled", false, 0, true);
            if (this._widgetSettings.ActiveWidgetSystemNames.Contains("InstantSearch"))
                return;
            this._widgetSettings.ActiveWidgetSystemNames.Add("Nop.Plugin.InstantSearch");
            await this._settingService.SaveSettingAsync(_widgetSettings, 0);
        }

        public override Type GetWidgetViewComponent(string widgetZone) => typeof(InstantSearchComponent);

        static InstantSearchPlugin()
        {
            var menuItem7SpikesList = new List<MenuItem7Spikes>();
            var menuItem7Spikes = new MenuItem7Spikes();
            menuItem7Spikes.SubMenuName = "InstantSearch.Admin.Submenus.Settings";
            menuItem7Spikes.SubMenuRelativePath = "InstantSearchAdmin/Settings";
            menuItem7SpikesList.Add(menuItem7Spikes);
            MenuItems = menuItem7SpikesList;
        }
    }
}
