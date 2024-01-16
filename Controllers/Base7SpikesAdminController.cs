﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Domain.Stores;
using Nop.Core.Infrastructure;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Stores;
using Nop.Web.Areas.Admin.Controllers;
using Nop.Plugin.InstantSearch.Areas.Admin.ControllerAttributes;
using Nop.Plugin.InstantSearch.Areas.Admin.Models;


#nullable enable
namespace Nop.Plugin.InstantSearch.Controllers
{
  [ManagePluginsAdminAuthorize("", false)]
  public abstract class Base7SpikesAdminController : BaseAdminController
  {
    private 
    #nullable disable
    IStoreMappingService _storeMappingService;
    private IStoreService _storeService;
    private INopFileProvider _nopFileProvider;
    private IStoreContext _storeContext;
    private ILocalizationService _localizationService;
    private INotificationService _notificationService;
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

    protected IStoreMappingService StoreMappingService
    {
      get
      {
        if (this._storeMappingService == null)
          this._storeMappingService = EngineContext.Current.Resolve<IStoreMappingService>((IServiceScope) null);
        return this._storeMappingService;
      }
    }

    protected IStoreService StoreService
    {
      get
      {
        if (this._storeService == null)
          this._storeService = EngineContext.Current.Resolve<IStoreService>((IServiceScope) null);
        return this._storeService;
      }
    }

    protected INopFileProvider NopFileProvider
    {
      get
      {
        if (this._nopFileProvider == null)
          this._nopFileProvider = EngineContext.Current.Resolve<INopFileProvider>((IServiceScope) null);
        return this._nopFileProvider;
      }
    }

    protected IStoreContext StoreContext
    {
      get
      {
        if (this._storeContext == null)
          this._storeContext = EngineContext.Current.Resolve<IStoreContext>((IServiceScope) null);
        return this._storeContext;
      }
    }

    protected ILocalizationService LocalizationService
    {
      get
      {
        if (this._localizationService == null)
          this._localizationService = EngineContext.Current.Resolve<ILocalizationService>((IServiceScope) null);
        return this._localizationService;
      }
    }

    private INotificationService NotificationService
    {
      get
      {
        if (this._notificationService == null)
          this._notificationService = EngineContext.Current.Resolve<INotificationService>((IServiceScope) null);
        return this._notificationService;
      }
    }

    protected void SuccessNotification(string message) => this.NotificationService.SuccessNotification(message, true);

    protected void ErrorNotification(string message) => this.NotificationService.ErrorNotification(message, true);

    [NonAction]
    protected async Task PrepareStoresMappingModelAsync<TEntity>(
      StoreMappingModel model,
      TEntity entity,
      bool excludeProperties)
      where TEntity : BaseEntity, IStoreMappingSupported
    {
      if (model == null)
        throw new ArgumentNullException(nameof (model));
      if (!excludeProperties && (object) (TEntity) entity != null)
      {
        StoreMappingModel storeMappingModel = model;
        storeMappingModel.SelectedStoreIds = (IList<int>) ((IEnumerable<int>) await this.StoreMappingService.GetStoresIdsWithAccessAsync<TEntity>(entity)).ToList<int>();
        storeMappingModel = (StoreMappingModel) null;
      }
      foreach (Store store in (IEnumerable<Store>) await this.StoreService.GetAllStoresAsync())
        model.AvailableStores.Add(new SelectListItem()
        {
          Text = store.Name,
          Value = ((BaseEntity) store).Id.ToString(),
          Selected = model.SelectedStoreIds.Contains(((BaseEntity) store).Id)
        });
    }

    [NonAction]
    protected async Task SaveStoreMappingsAsync<TEntity>(TEntity entity, StoreMappingModel model) where TEntity : BaseEntity, IStoreMappingSupported
    {
      ((TEntity) entity).LimitedToStores = model.SelectedStoreIds.Any<int>();
      IList<StoreMapping> existingStoreMappings = await this.StoreMappingService.GetStoreMappingsAsync<TEntity>(entity);
      foreach (Store store1 in (IEnumerable<Store>) await this.StoreService.GetAllStoresAsync())
      {
        Store store = store1;
        if (model.SelectedStoreIds != null && model.SelectedStoreIds.Contains(((BaseEntity) store).Id))
        {
          if (existingStoreMappings.Count<StoreMapping>((Func<StoreMapping, bool>) (sm => sm.StoreId == ((BaseEntity) store).Id)) == 0)
            await this.StoreMappingService.InsertStoreMappingAsync<TEntity>(entity, ((BaseEntity) store).Id);
        }
        else
        {
          StoreMapping storeMapping = existingStoreMappings.FirstOrDefault<StoreMapping>((Func<StoreMapping, bool>) (sm => sm.StoreId == ((BaseEntity) store).Id));
          if (storeMapping != null)
            await this.StoreMappingService.DeleteStoreMappingAsync(storeMapping);
        }
      }
      existingStoreMappings = (IList<StoreMapping>) null;
    }

    [NonAction]
    protected void PrepareCopyModel<TEntity>(
      CopyModel copyModel,
      TEntity entity,
      string actionName,
      string contollerName,
      string newName,
      bool copyConditions = false,
      bool copyMappings = false,
      bool copyScheduling = false)
      where TEntity : BaseEntity
    {
      copyModel.Id = entity.Id;
      copyModel.Name = newName;
      copyModel.ActionName = actionName;
      copyModel.ControllerName = contollerName;
      copyModel.SupportCopyConditions = copyConditions;
      copyModel.CopyConditions = copyConditions;
      copyModel.SupportCopyMappings = copyMappings;
      copyModel.CopyMappings = copyMappings;
      copyModel.SupportCopyScheduling = copyScheduling;
      copyModel.CopyScheduling = copyScheduling;
    }

    protected JsonResult EmptyJson => new JsonResult((object) null);
  }
}
