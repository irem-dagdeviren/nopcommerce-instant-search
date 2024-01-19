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
    private IUrlRecordService _urlRecordService;

        protected IUrlRecordService UrlRecordService
        {
            get
            {
            if (this._urlRecordService == null)
                this._urlRecordService = EngineContext.Current.Resolve<IUrlRecordService>((IServiceScope) null);
            return this._urlRecordService;
            }
        }
    }
}
