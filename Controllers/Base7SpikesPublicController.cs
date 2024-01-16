using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Caching;
using Nop.Core.Infrastructure;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Localization;
using Nop.Services.Seo;
using Nop.Web.Controllers;

namespace Nop.Plugin.InstantSearch.Controllers
{
  public abstract class Base7SpikesPublicController : BasePublicController
  {
    private ILocalizationService _localizationService;
    private IUrlRecordService _urlRecordService;
    private IGenericAttributeService _genericAttributeService;
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

    protected IGenericAttributeService GenericAttributeService
    {
      get
      {
        if (this._genericAttributeService == null)
          this._genericAttributeService = EngineContext.Current.Resolve<IGenericAttributeService>((IServiceScope) null);
        return this._genericAttributeService;
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
