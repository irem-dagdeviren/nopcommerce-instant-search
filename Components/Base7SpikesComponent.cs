using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Caching;
using Nop.Core.Infrastructure;
using Nop.Data;
using Nop.Services.Customers;
using Nop.Services.Localization;
using Nop.Services.Seo;
using Nop.Web.Framework.Components;
using Nop.Web.Models.Catalog;

namespace Nop.Plugin.InstantSearch.Components
{
    public abstract class Base7SpikesComponent : NopViewComponent
  {
    private ILocalizationService _localizationService;
    private IUrlRecordService _urlRecordService;
    private ICustomerService _customerService;
    private IStaticCacheManager _staticCacheManager;

    protected ILocalizationService LocalizationService
    {
      get
      {
        if (this._localizationService == null)
          this._localizationService = EngineContext.Current.Resolve<ILocalizationService>((IServiceScope) null);
        return this._localizationService;
      }
    }

    protected IUrlRecordService UrlRecordService
    {
      get
      {
        if (this._urlRecordService == null)
          this._urlRecordService = EngineContext.Current.Resolve<IUrlRecordService>((IServiceScope) null);
        return this._urlRecordService;
      }
    }

    protected ICustomerService CustomerService
    {
      get
      {
        if (this._customerService == null)
          this._customerService = EngineContext.Current.Resolve<ICustomerService>((IServiceScope) null);
        return this._customerService;
      }
    }

    protected IStaticCacheManager StaticCacheManager
    {
      get
      {
        if (this._staticCacheManager == null)
          this._staticCacheManager = EngineContext.Current.Resolve<IStaticCacheManager>((IServiceScope) null);
        return this._staticCacheManager;
      }
    }
  }
}
