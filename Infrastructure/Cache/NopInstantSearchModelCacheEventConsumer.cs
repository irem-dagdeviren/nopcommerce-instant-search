using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Caching;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Vendors;
using Nop.Core.Events;
using Nop.Core.Infrastructure;
using Nop.Services.Events;

namespace Nop.Plugin.InstantSearch.Infrastructure.Cache
{
  public class NopInstantSearchModelCacheEventConsumer : 
    IConsumer<EntityInsertedEvent<Category>>,
    IConsumer<EntityUpdatedEvent<Category>>,
    IConsumer<EntityDeletedEvent<Category>>,
    IConsumer<EntityInsertedEvent<Manufacturer>>,
    IConsumer<EntityUpdatedEvent<Manufacturer>>,
    IConsumer<EntityDeletedEvent<Manufacturer>>,
    IConsumer<EntityInsertedEvent<Vendor>>,
    IConsumer<EntityUpdatedEvent<Vendor>>,
    IConsumer<EntityDeletedEvent<Vendor>>
  {
    private IStaticCacheManager _staticCacheManager;

    private IStaticCacheManager StaticCacheManager
    {
      get
      {
        if (this._staticCacheManager == null)
          this._staticCacheManager = EngineContext.Current.Resolve<IStaticCacheManager>((IServiceScope) null);
        return this._staticCacheManager;
      }
    }

    public async Task HandleEventAsync(EntityInsertedEvent<Category> eventMessage) => await this.StaticCacheManager.RemoveByPrefixAsync("nop.pres.nop.instant.search", Array.Empty<object>());

    public async Task HandleEventAsync(EntityUpdatedEvent<Category> eventMessage) => await this.StaticCacheManager.RemoveByPrefixAsync("nop.pres.nop.instant.search", Array.Empty<object>());

    public async Task HandleEventAsync(EntityDeletedEvent<Category> eventMessage) => await this.StaticCacheManager.RemoveByPrefixAsync("nop.pres.nop.instant.search", Array.Empty<object>());

    public async Task HandleEventAsync(EntityInsertedEvent<Manufacturer> eventMessage) => await this.StaticCacheManager.RemoveByPrefixAsync("nop.pres.nop.instant.search", Array.Empty<object>());

    public async Task HandleEventAsync(EntityUpdatedEvent<Manufacturer> eventMessage) => await this.StaticCacheManager.RemoveByPrefixAsync("nop.pres.nop.instant.search", Array.Empty<object>());

    public async Task HandleEventAsync(EntityDeletedEvent<Manufacturer> eventMessage) => await this.StaticCacheManager.RemoveByPrefixAsync("nop.pres.nop.instant.search", Array.Empty<object>());

    public async Task HandleEventAsync(EntityInsertedEvent<Vendor> eventMessage) => await this.StaticCacheManager.RemoveByPrefixAsync("nop.pres.nop.instant.search", Array.Empty<object>());

    public async Task HandleEventAsync(EntityUpdatedEvent<Vendor> eventMessage) => await this.StaticCacheManager.RemoveByPrefixAsync("nop.pres.nop.instant.search", Array.Empty<object>());

    public async Task HandleEventAsync(EntityDeletedEvent<Vendor> eventMessage) => await this.StaticCacheManager.RemoveByPrefixAsync("nop.pres.nop.instant.search", Array.Empty<object>());
  }
}
