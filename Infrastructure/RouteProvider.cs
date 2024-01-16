using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core;
using Nop.Core.Domain.Configuration;
using Nop.Core.Domain.Stores;
using Nop.Core.Infrastructure;
using Nop.Services.Configuration;
using Nop.Services.Stores;
using Nop.Plugin.InstantSearch.Routing;

namespace Nop.Plugin.InstantSearch.Infrastructure
{
  public class RouteProvider : BaseRouteProvider
  {    
    protected override async Task RegisterRoutesAccessibleByNameAsync(
        IEndpointRouteBuilder endpointRouteBuilder)
    {
            RouteProvider routeProvider = this;
            endpointRouteBuilder.MapControllerRoute(name: "InstantSearchFor",
                     pattern: "InstantSearchFor",
                     defaults: new { controller = "InstantSearch", action = "InstantSearchFor" });

        }

        protected override  string PluginSystemName => "InstantSearch";

    protected override  async Task CheckBackwardCompatibilityAsync()
    {
      ISettingService settingService = EngineContext.Current.Resolve<ISettingService>((IServiceScope) null);
      IStoreService storeService = EngineContext.Current.Resolve<IStoreService>((IServiceScope) null);
      if (await settingService.GetSettingByKeyAsync<bool>("InstantSearchCommonSettings.LoadStoreSettingsOnLoad", true, 0, false))
      {
        List<int> storeIds = new List<int>() { 0 };
        List<int> intList = storeIds;
        intList.AddRange((await storeService.GetAllStoresAsync()).Select<Store, int>((Func<Store, int>) (store => ((BaseEntity) store).Id)));
        intList = (List<int>) null;
        foreach (int storeId in storeIds)
        {
          if (await settingService.GetSettingByKeyAsync<bool>("instantsearchsettings.enablecategorysearch", false, storeId, false))
            await settingService.SetSettingAsync<int>("instantsearchsettings.searchoption", 1, storeId, true);
          Setting settingAsync = await settingService.GetSettingAsync("instantsearchsettings.enablecategorysearch", storeId, false);
          if (settingAsync != null)
            await settingService.DeleteSettingAsync(settingAsync);
        }
        storeIds = (List<int>) null;
      }
      await base.CheckBackwardCompatibilityAsync();
      settingService = (ISettingService) null;
      storeService = (IStoreService) null;
    }

    public  RouteProvider()
      : base(true)
    {
    }
  }
}
