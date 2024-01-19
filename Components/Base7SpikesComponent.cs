using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Caching;
using Nop.Core.Infrastructure;
using Nop.Services.Customers;
using Nop.Services.Localization;
using Nop.Services.Seo;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.InstantSearch.Components
{
    public abstract class Base7SpikesComponent : NopViewComponent
    {
        private IStaticCacheManager _staticCacheManager;

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
