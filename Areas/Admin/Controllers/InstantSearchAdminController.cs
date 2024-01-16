using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Cms;
using Nop.Core.Domain.Logging;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Security;
using Nop.Services.Stores;
using Nop.Web.Framework.Controllers;
using Nop.Plugin.InstantSearch.Areas.Admin.ControllerAttributes;
using Nop.Plugin.InstantSearch.Areas.Admin.Helpers;
using Nop.Plugin.InstantSearch.Areas.Admin.Extensions;
using Nop.Plugin.InstantSearch.Areas.Admin.Models;
using Nop.Plugin.InstantSearch.Domain;
using System.Linq.Expressions;
using Nop.Plugin.InstantSearch.Controllers;

namespace Nop.Plugin.InstantSearch.Areas.Admin.Controllers
{
  [ManagePluginsAdminAuthorize("Nop.Plugin.InstantSearch", false)]

    public class InstantSearchAdminController : Base7SpikesAdminController
    {
        private readonly IPermissionService _permissionService;
        private readonly ISettingService _settingService;
        private readonly ILocalizationService _localizationService;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly WidgetSettings _widgetSettings;
        private readonly CatalogSettings _catalogSettings;
        private readonly IStoreService _storeService;
        private readonly IWorkContext _workContext;

        public InstantSearchAdminController(
          IPermissionService permissionService,
          ISettingService settingService,
          ILocalizationService localizationService,
          ICustomerActivityService customerActivityService,
          WidgetSettings widgetSettings,
          CatalogSettings catalogSettings,
          IStoreService storeService,
          IWorkContext workContext)
        {
            this._permissionService = permissionService;
            this._settingService = settingService;
            this._localizationService = localizationService;
            this._customerActivityService = customerActivityService;
            this._widgetSettings = widgetSettings;
            this._catalogSettings = catalogSettings;
            this._storeService = storeService;
            this._workContext = workContext;
        }


        public async Task<ActionResult> Settings()
    {
            InstantSearchAdminController searchAdminController = this;
            int storeScope = await searchAdminController.StoreContext.GetActiveStoreScopeConfigurationAsync();
            InstantSearchSettings instantSearchSettings = await searchAdminController._settingService.LoadSettingAsync<InstantSearchSettings>(storeScope);
            InstantSearchSettingsModel model = instantSearchSettings.ToModel();
            CatalogSettings catalogSetting = await searchAdminController._settingService.LoadSettingAsync<CatalogSettings>(storeScope);
            bool isDefaultAutocompleteEnabled = catalogSetting.ProductSearchAutoCompleteEnabled;
            await searchAdminController.PopulateAvailableSearchOptionsAsync(model);
            await searchAdminController.PopulateAvailableProductSortOptionsAsync(model);
            model.ActiveStoreScopeConfiguration = storeScope;
            model.Enable = model.Enable && searchAdminController._widgetSettings.ActiveWidgetSystemNames.Contains("InstantSearch") && !isDefaultAutocompleteEnabled;
            model.MinKeywordLength = catalogSetting.ProductSearchTermMinimumLength;
            if (storeScope > 0)
            {
                StoreScopeSettingsHelper<InstantSearchSettings> storeScopeSetting = new StoreScopeSettingsHelper<InstantSearchSettings>(instantSearchSettings, storeScope, searchAdminController._settingService);
                StoreScopeSettingsHelper<CatalogSettings> catalogStoreScopeSetting = new StoreScopeSettingsHelper<CatalogSettings>(catalogSetting, storeScope, searchAdminController._settingService);
                InstantSearchSettingsModel searchSettingsModel = model;
                StoreScopeSettingsHelper<InstantSearchSettings> scopeSettingsHelper1 = storeScopeSetting;
                Expression<Func<InstantSearchSettings, bool>> expression1 = (Expression<Func<InstantSearchSettings, bool>>)(x => x.Enable);
                searchSettingsModel.Enable_OverrideForStore = await scopeSettingsHelper1.SettingExistsAsync<bool>(expression1);
                searchSettingsModel = (InstantSearchSettingsModel)null;
                searchSettingsModel = model;
                StoreScopeSettingsHelper<InstantSearchSettings> scopeSettingsHelper2 = storeScopeSetting;
                Expression<Func<InstantSearchSettings, int>> expression2 = (Expression<Func<InstantSearchSettings, int>>)(x => x.SearchOption);
                searchSettingsModel.SearchOption_OverrideForStore = await scopeSettingsHelper2.SettingExistsAsync<int>(expression2);
                searchSettingsModel = (InstantSearchSettingsModel)null;
                searchSettingsModel = model;
                StoreScopeSettingsHelper<InstantSearchSettings> scopeSettingsHelper3 = storeScopeSetting;
                Expression<Func<InstantSearchSettings, int>> expression3 = (Expression<Func<InstantSearchSettings, int>>)(x => x.NumberOfProducts);
                searchSettingsModel.NumberOfProducts_OverrideForStore = await scopeSettingsHelper3.SettingExistsAsync<int>(expression3);
                searchSettingsModel = (InstantSearchSettingsModel)null;
                searchSettingsModel = model;
                StoreScopeSettingsHelper<InstantSearchSettings> scopeSettingsHelper4 = storeScopeSetting;
                Expression<Func<InstantSearchSettings, int>> expression4 = (Expression<Func<InstantSearchSettings, int>>)(x => x.DefaultProductSortOption);
                searchSettingsModel.DefaultProductSortOption_OverrideForStore = await scopeSettingsHelper4.SettingExistsAsync<int>(expression4);
                searchSettingsModel = (InstantSearchSettingsModel)null;
                searchSettingsModel = model;
                StoreScopeSettingsHelper<InstantSearchSettings> scopeSettingsHelper5 = storeScopeSetting;
                Expression<Func<InstantSearchSettings, int>> expression5 = (Expression<Func<InstantSearchSettings, int>>)(x => x.PictureSize);
                searchSettingsModel.PictureSize_OverrideForStore = await scopeSettingsHelper5.SettingExistsAsync<int>(expression5);
                searchSettingsModel = (InstantSearchSettingsModel)null;
                searchSettingsModel = model;
                StoreScopeSettingsHelper<InstantSearchSettings> scopeSettingsHelper6 = storeScopeSetting;
                Expression<Func<InstantSearchSettings, bool>> expression6 = (Expression<Func<InstantSearchSettings, bool>>)(x => x.ShowSku);
                searchSettingsModel.ShowSku_OverrideForStore = await scopeSettingsHelper6.SettingExistsAsync<bool>(expression6);
                searchSettingsModel = (InstantSearchSettingsModel)null;
                searchSettingsModel = model;
                StoreScopeSettingsHelper<InstantSearchSettings> scopeSettingsHelper7 = storeScopeSetting;
                Expression<Func<InstantSearchSettings, bool>> expression7 = (Expression<Func<InstantSearchSettings, bool>>)(x => x.HighlightFirstFoundElementToBeSelected);
                searchSettingsModel.HighlightFirstFoundElementToBeSelected_OverrideForStore = await scopeSettingsHelper7.SettingExistsAsync<bool>(expression7);
                searchSettingsModel = (InstantSearchSettingsModel)null;
                searchSettingsModel = model;
                StoreScopeSettingsHelper<InstantSearchSettings> scopeSettingsHelper8 = storeScopeSetting;
                Expression<Func<InstantSearchSettings, bool>> expression8 = (Expression<Func<InstantSearchSettings, bool>>)(x => x.SearchDescriptions);
                searchSettingsModel.SearchDescriptions_OverrideForStore = await scopeSettingsHelper8.SettingExistsAsync<bool>(expression8);
                searchSettingsModel = (InstantSearchSettingsModel)null;
                searchSettingsModel = model;
                StoreScopeSettingsHelper<InstantSearchSettings> scopeSettingsHelper9 = storeScopeSetting;
                Expression<Func<InstantSearchSettings, bool>> expression9 = (Expression<Func<InstantSearchSettings, bool>>)(x => x.SearchProductTags);
                searchSettingsModel.SearchProductTags_OverrideForStore = await scopeSettingsHelper9.SettingExistsAsync<bool>(expression9);
                searchSettingsModel = (InstantSearchSettingsModel)null;
                searchSettingsModel = model;
                StoreScopeSettingsHelper<InstantSearchSettings> scopeSettingsHelper10 = storeScopeSetting;
                Expression<Func<InstantSearchSettings, bool>> expression10 = (Expression<Func<InstantSearchSettings, bool>>)(x => x.VisibleIndividuallyOnly);
                searchSettingsModel.VisibleIndividuallyOnly_OverrideForStore = await scopeSettingsHelper10.SettingExistsAsync<bool>(expression10);
                searchSettingsModel = (InstantSearchSettingsModel)null;
                searchSettingsModel = model;
                StoreScopeSettingsHelper<InstantSearchSettings> scopeSettingsHelper11 = storeScopeSetting;
                Expression<Func<InstantSearchSettings, int>> expression11 = (Expression<Func<InstantSearchSettings, int>>)(x => x.NumberOfVisibleProducts);
                searchSettingsModel.NumberOfVisibleProducts_OverrideForStore = await scopeSettingsHelper11.SettingExistsAsync<int>(expression11);
                searchSettingsModel = (InstantSearchSettingsModel)null;
                searchSettingsModel = model;
                StoreScopeSettingsHelper<CatalogSettings> scopeSettingsHelper12 = catalogStoreScopeSetting;

                ParameterExpression parameterExpression = Expression.Parameter(typeof(CatalogSettings), "ProductSearchTermMinimumLength");
                Expression<Func<CatalogSettings, int>> expression12 = Expression.Lambda<Func<CatalogSettings, int>>(
                    Expression.Property(parameterExpression, 
                    typeof(CatalogSettings).GetProperty("ProductSearchTermMinimumLength")),
                    parameterExpression
                );

                searchSettingsModel.MinKeywordLength_OverrideForStore = await scopeSettingsHelper12.SettingExistsAsync<int>(expression12);
                searchSettingsModel = (InstantSearchSettingsModel)null;
                storeScopeSetting = (StoreScopeSettingsHelper<InstantSearchSettings>)null;
                catalogStoreScopeSetting = (StoreScopeSettingsHelper<CatalogSettings>)null;
      }
            model.IsTrialVersion = false;
            ActionResult actionResult = (ActionResult)((Controller)searchAdminController).View("~/Plugins/InstantSearch/Areas/Admin/Views/InstantSearchAdmin/Settings.cshtml", (object)model);
            instantSearchSettings = (InstantSearchSettings)null;
            model = (InstantSearchSettingsModel)null;
            catalogSetting = (CatalogSettings)null;
            return actionResult;
        }

        [HttpPost]
        public async Task<ActionResult> Settings(
      InstantSearchSettingsModel model,
      bool returnPartialView)
        {
            InstantSearchAdminController searchAdminController = this;
            int storeScope = await searchAdminController.StoreContext.GetActiveStoreScopeConfigurationAsync();
            if (model.Enable)
            {
                if (!searchAdminController._widgetSettings.ActiveWidgetSystemNames.Contains("InstantSearch"))
                {
                    searchAdminController._widgetSettings.ActiveWidgetSystemNames.Add("Nop.Plugin.InstantSearch");
                    await searchAdminController._settingService.SaveSettingAsync<WidgetSettings>(searchAdminController._widgetSettings, 0);
                }
                if ((await searchAdminController._settingService.LoadSettingAsync<CatalogSettings>(storeScope)).ProductSearchAutoCompleteEnabled)
                {
                    searchAdminController._catalogSettings.ProductSearchAutoCompleteEnabled = false;
                    ParameterExpression parameterExpression = Expression.Parameter(typeof(CatalogSettings), "ProductSearchAutoCompleteEnabled");
                    await searchAdminController._settingService.SaveSettingAsync<CatalogSettings, bool>(
                        searchAdminController._catalogSettings,
                        Expression.Lambda<Func<CatalogSettings, bool>>(
                            Expression.Property(parameterExpression, typeof(CatalogSettings).GetProperty("ProductSearchAutoCompleteEnabled")),
                            parameterExpression),
                        storeScope,
                        true
                    );
                }
            }
            CatalogSettings catalogSettings = await searchAdminController._settingService.LoadSettingAsync<CatalogSettings>(storeScope);
            catalogSettings.ProductSearchTermMinimumLength = model.MinKeywordLength;

            ParameterExpression parameterExpression1 = Expression.Parameter(typeof(CatalogSettings), "CatalogSettings");

            await new StoreScopeSettingsHelper<CatalogSettings>(
                catalogSettings, storeScope, searchAdminController._settingService).SaveStoreSettingAsync<int>((
                model.MinKeywordLength_OverrideForStore ? 1 : 0) != 0,
                Expression.Lambda<Func<CatalogSettings, int>>(
                    (Expression)Expression.Property((Expression)parameterExpression1,
                    typeof(CatalogSettings).GetProperty("ProductSearchTermMinimumLength")),
                    parameterExpression1));

            StoreScopeSettingsHelper<InstantSearchSettings> storeScopeSetting = new StoreScopeSettingsHelper<InstantSearchSettings>(model.ToEntity(), storeScope, searchAdminController._settingService);
            await storeScopeSetting.SaveStoreSettingAsync<bool>((model.Enable_OverrideForStore ? 1 : 0) != 0, (Expression<Func<InstantSearchSettings, bool>>)(x => x.Enable));
            await storeScopeSetting.SaveStoreSettingAsync<int>((model.SearchOption_OverrideForStore ? 1 : 0) != 0, (Expression<Func<InstantSearchSettings, int>>)(x => x.SearchOption));
            await storeScopeSetting.SaveStoreSettingAsync<int>((model.NumberOfProducts_OverrideForStore ? 1 : 0) != 0, (Expression<Func<InstantSearchSettings, int>>)(x => x.NumberOfProducts));
            await storeScopeSetting.SaveStoreSettingAsync<int>((model.DefaultProductSortOption_OverrideForStore ? 1 : 0) != 0, (Expression<Func<InstantSearchSettings, int>>)(x => x.DefaultProductSortOption));
            await storeScopeSetting.SaveStoreSettingAsync<int>((model.PictureSize_OverrideForStore ? 1 : 0) != 0, (Expression<Func<InstantSearchSettings, int>>)(x => x.PictureSize));
            await storeScopeSetting.SaveStoreSettingAsync<bool>((model.ShowSku_OverrideForStore ? 1 : 0) != 0, (Expression<Func<InstantSearchSettings, bool>>)(x => x.ShowSku));
            await storeScopeSetting.SaveStoreSettingAsync<bool>((model.HighlightFirstFoundElementToBeSelected_OverrideForStore ? 1 : 0) != 0, (Expression<Func<InstantSearchSettings, bool>>)(x => x.HighlightFirstFoundElementToBeSelected));
            await storeScopeSetting.SaveStoreSettingAsync<bool>((model.SearchDescriptions_OverrideForStore ? 1 : 0) != 0, (Expression<Func<InstantSearchSettings, bool>>)(x => x.SearchDescriptions));
            await storeScopeSetting.SaveStoreSettingAsync<bool>((model.SearchProductTags_OverrideForStore ? 1 : 0) != 0, (Expression<Func<InstantSearchSettings, bool>>)(x => x.SearchProductTags));
            await storeScopeSetting.SaveStoreSettingAsync<bool>((model.VisibleIndividuallyOnly_OverrideForStore ? 1 : 0) != 0, (Expression<Func<InstantSearchSettings, bool>>)(x => x.VisibleIndividuallyOnly));
            await storeScopeSetting.SaveStoreSettingAsync<int>((model.NumberOfVisibleProducts_OverrideForStore ? 1 : 0) != 0, (Expression<Func<InstantSearchSettings, int>>)(x => x.NumberOfVisibleProducts));
            await searchAdminController._settingService.ClearCacheAsync();
            ICustomerActivityService icustomerActivityService = searchAdminController._customerActivityService;
            ActivityLog activityLog = await icustomerActivityService.InsertActivityAsync("EditInstantSearchSettings", await searchAdminController._localizationService.GetResourceAsync("ActivityLog.EditInstantSearchSettings"), (BaseEntity)null);
            icustomerActivityService = (ICustomerActivityService)null;
            string resourceAsync = await searchAdminController._localizationService.GetResourceAsync("Admin.Configuration.Updated");
            searchAdminController.SuccessNotification(resourceAsync);
            ((BaseController)searchAdminController).SaveSelectedTabName("", true);
            ActionResult action = (ActionResult)((ControllerBase)searchAdminController).RedirectToAction(nameof(Settings));
            storeScopeSetting = (StoreScopeSettingsHelper<InstantSearchSettings>)null;
            return action;
        }

        private async Task PopulateAvailableProductSortOptionsAsync(InstantSearchSettingsModel model)
    {
      foreach (ProductSortingEnum enumValue in Enum.GetValues(typeof (ProductSortingEnum)))
      {
        string localizedEnumAsync = await this._localizationService.GetLocalizedEnumAsync<ProductSortingEnum>(enumValue, new int?());
        model.AvailableProductSortOptions.Add(new SelectListItem()
        {
          Text = localizedEnumAsync,
          Value = enumValue.ToString(),
          Selected = enumValue.Equals(model.DefaultProductSortOption)

        });
      }
    }

    private async Task PopulateAvailableSearchOptionsAsync(InstantSearchSettingsModel model)
    {
      foreach (Domain.Enums.SearchOption enumValue in Enum.GetValues(typeof (Domain.Enums.SearchOption)))
      {
        string localizedEnumAsync = await this._localizationService.GetLocalizedEnumAsync<Domain.Enums.SearchOption>(enumValue, new int?());
        model.AvailableSearchOptions.Add(new SelectListItem()
        {
          Text = localizedEnumAsync,
          Value = ((int) enumValue).ToString()
        });
      }
    }
  }
}
