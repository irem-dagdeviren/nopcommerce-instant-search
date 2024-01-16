using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;
using Nop.Services.Cms;
using Nop.Services.Plugins;


#nullable enable
namespace Nop.Plugin.InstantSearch.Plugin
{
  public abstract class BaseAdminWidgetPlugin7Spikes : BaseAdminPlugin7Spikes, IWidgetPlugin, IPlugin
  {
    private readonly IInstallHelper _installHelper;

    public BaseAdminWidgetPlugin7Spikes(
      List<MenuItem7Spikes> menuItems,
      string pluginAdminMenuName,
      string pluginFolderName,
      bool isTrialVersion = false,
      string pluginUrlInStore = "")
      : base(menuItems, pluginAdminMenuName, pluginFolderName, isTrialVersion, pluginUrlInStore)
    {
      this._installHelper = EngineContext.Current.Resolve<IInstallHelper>( );
    }

    public async Task<IList<string>> GetWidgetZonesAsync()
    {
      BaseAdminWidgetPlugin7Spikes widgetPlugin7Spikes = this;
            List<string> list = (await widgetPlugin7Spikes._installHelper.GetSupportedWidgetZonesAsync(widgetPlugin7Spikes.PluginFolderName)).ToList<string>();

            list.AddRange((IEnumerable<string>)widgetPlugin7Spikes.GetSupportedAdminWidgetZones());
            return (IList<string>)list;

        }

        public bool HideInWidgetList => false;

    protected virtual IList<string> GetSupportedAdminWidgetZones() => (IList<string>) new List<string>();

    public abstract Type GetWidgetViewComponent(string widgetZone);
  }
}
