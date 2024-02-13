using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Stores;
using Nop.Web.Framework.Components;
using Nop.Web.Models.Catalog;
using Nop.Plugin.InstantSearch.Theme;
using Nop.Plugin.InstantSearch.Domain;
using Nop.Plugin.InstantSearch.Infrastructure.Constants;
using Nop.Plugin.InstantSearch.Models;
using Nop.Plugin.InstantSearch.Services.Catalog;
using Nop.Plugin.InstantSearch.Services.Helpers;
using Nop.Data;
using Nop.Core.Domain.Vendors;
using Microsoft.AspNetCore.Http;

namespace Nop.Plugin.InstantSearch.Components
{
    [ViewComponent(Name = "InstantSearch")]
    public class InstantSearchComponent : Base7SpikesComponent
    {
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly ICategoryService7Spikes _categoryServiceSevenSpikes;
        private readonly IAclHelper _aclHelper;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly CatalogSettings _catalogSettings;
        private readonly DuzeySearchSettings _instantSearchSettings;
        private readonly IRepository<Manufacturer> _manufacturerRepository;
        private readonly IRepository<Vendor> _vendorRepository;

        public InstantSearchComponent(
            IWorkContext workContext,
            IStoreContext storeContext,
            ICategoryService7Spikes categoryServiceSevenSpikes,
            IAclHelper aclHelper,
            IStaticCacheManager staticCacheManager,
            CatalogSettings catalogSettings,
            DuzeySearchSettings instantSearchSettings,
            IRepository<Manufacturer> manufacturerRepository,
            IRepository<Vendor> vendorRepository
        )
        {
            this._storeContext = storeContext;
            this._categoryServiceSevenSpikes = categoryServiceSevenSpikes;
            this._aclHelper = aclHelper;
            this._workContext = workContext;
            this._staticCacheManager = staticCacheManager;
            this._catalogSettings = catalogSettings;
            this._instantSearchSettings = instantSearchSettings;
            this._manufacturerRepository = manufacturerRepository;
            this._vendorRepository = vendorRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            InstantSearchComponent instantSearchComponent = this;
            if (instantSearchComponent._catalogSettings.ProductSearchAutoCompleteEnabled || !instantSearchComponent._instantSearchSettings.Enable)
                return (IViewComponentResult)((ViewComponent)instantSearchComponent).Content("");
            InstantSearchModel instantSearchModel1 = new InstantSearchModel();
            instantSearchModel1.MinKeywordLength = instantSearchComponent._catalogSettings.ProductSearchTermMinimumLength;
            instantSearchModel1.DefaultProductSortOption = instantSearchComponent._instantSearchSettings.DefaultProductSortOption;
            instantSearchModel1.SearchInProductDescriptions = instantSearchComponent._instantSearchSettings.SearchDescriptions;
            instantSearchModel1.HighlightFirstFoundElementToBeSelected = instantSearchComponent._instantSearchSettings.HighlightFirstFoundElementToBeSelected;
            instantSearchModel1.ShowSku = instantSearchComponent._instantSearchSettings.ShowSku;
            InstantSearchModel instantSearchModel2 = instantSearchModel1;
            instantSearchModel2.Theme = await ThemeHelper.GetPluginThemeAsync("Nop.Plugin.InstantSearch");
            instantSearchModel1.NumberOfVisibleProducts = instantSearchComponent._instantSearchSettings.NumberOfVisibleProducts;
            instantSearchModel1.MaximumNumberOfProducts = instantSearchComponent._instantSearchSettings.NumberOfProducts;
            InstantSearchModel model = instantSearchModel1;
            instantSearchModel2 = (InstantSearchModel)null;
            instantSearchModel1 = (InstantSearchModel)null;
            if (instantSearchComponent._instantSearchSettings.SearchOption == 1)
            {
                instantSearchModel1 = model;
                instantSearchModel1.TopLevelCategories = await instantSearchComponent.GetTopLevelCategoriesAsync();
                instantSearchModel1 = (InstantSearchModel)null;
            }
            else if (instantSearchComponent._instantSearchSettings.SearchOption == 5)
            {
                instantSearchModel1 = model;
                instantSearchModel1.Manufacturers = await instantSearchComponent.GetAllManufacturersAsync();
                instantSearchModel1 = (InstantSearchModel)null;
            }
            else if (instantSearchComponent._instantSearchSettings.SearchOption == 10)
            {
                instantSearchModel1 = model;
                instantSearchModel1.Vendors = await instantSearchComponent.GetAllVendorsAsync();
                instantSearchModel1 = (InstantSearchModel)null;
            }
            bool isMobileView = IsMobileView(HttpContext);

            // Choose the appropriate view based on the mobile status
            string viewPath = isMobileView
                ? "~/Plugins/InstantSearch/Views/Components/InstantSearch/InstantSearchMobile.cshtml"
                : "~/Plugins/InstantSearch/Views/Components/InstantSearch/InstantSearch.cshtml";

            // Return the corresponding view
            return (IViewComponentResult)((NopViewComponent)instantSearchComponent).View(viewPath, model);
        }

        private bool IsMobileView(HttpContext context)
        {
            // Check if the "viewportWidth" cookie exists
            if (context.Request.Cookies.ContainsKey("viewportWidth"))
            {
                // Get the viewport width from the cookie
                var viewportWidth = int.Parse(context.Request.Cookies["viewportWidth"]);

                // Assume a threshold width for mobile view
                int thresholdWidthForMobileView = 1025; // Example threshold, adjust as needed

                // Return true if viewport width is less than the threshold, indicating a mobile view
                return viewportWidth < thresholdWidthForMobileView;
            }

            // Default to non-mobile view if the "viewportWidth" cookie is not available
            return false;
        }

        private async Task<IList<CategorySimpleModel>> GetTopLevelCategoriesAsync()
        {
            InstantSearchComponent instantSearchComponent = this;
            IStaticCacheManager istaticCacheManager = instantSearchComponent.StaticCacheManager;
            CacheKey cacheKey = CacheKeys.NOP_INSTANT_SEARCH_CATEGORIES_MODEL_KEY;
            object obj1 = (object)((BaseEntity)await instantSearchComponent._workContext.GetWorkingLanguageAsync()).Id;
            object obj2 = (object)await instantSearchComponent._aclHelper.GetAllowedCustomerRolesIdsAsync();
            Store currentStoreAsync = await instantSearchComponent._storeContext.GetCurrentStoreAsync();
            CacheKey cacheKey1 = istaticCacheManager.PrepareKeyForDefaultCache(cacheKey, new object[3]
            {
                obj1,
                obj2,
                (object) ((BaseEntity) currentStoreAsync).Id
            });
            istaticCacheManager = (IStaticCacheManager)null;
            cacheKey = (CacheKey)null;
            obj1 = (object)null;
            obj2 = (object)null;
            return await instantSearchComponent._staticCacheManager.GetAsync<IList<CategorySimpleModel>>(
                cacheKey1, async () =>
                {
                    var categories = await _categoryServiceSevenSpikes.GetCategoriesByParentCategoryIdAsync(0);

                    var topLevelCategories = categories.Select(category => new CategorySimpleModel

                    {
                        Id = category.Id,
                        Name = category.Name,
                        IncludeInTopMenu = category.IncludeInTopMenu                     // Map other properties as needed
                    })
                    .ToList();
                    return topLevelCategories;
                });
            }
     
        private async Task<IList<ManufacturerSimpleModel>> GetAllManufacturersAsync()
        {
            InstantSearchComponent instantSearchComponent = this;
            IStaticCacheManager istaticCacheManager = instantSearchComponent.StaticCacheManager;
            CacheKey cacheKey = CacheKeys.NOP_INSTANT_SEARCH_MANUFACTURERS_MODEL_KEY;
            object obj1 = (object) ((BaseEntity) await instantSearchComponent._workContext.GetWorkingLanguageAsync()).Id;
            object obj2 = (object) await instantSearchComponent._aclHelper.GetAllowedCustomerRolesIdsAsync();
            Store currentStoreAsync = await instantSearchComponent._storeContext.GetCurrentStoreAsync();
            CacheKey cacheKey1 = istaticCacheManager.PrepareKeyForDefaultCache(cacheKey, new object[3]
            {
                obj1,
                obj2,
                (object) ((BaseEntity) currentStoreAsync).Id
            });
            istaticCacheManager = (IStaticCacheManager)null;
            cacheKey = (CacheKey)null;
            obj1 = (object)null;
            obj2 = (object)null;
            return await instantSearchComponent._staticCacheManager.GetAsync<IList<ManufacturerSimpleModel>>(
            cacheKey1, async () =>
            {
                IList<Manufacturer> manufacturers = _manufacturerRepository.Table.ToList();
                IList<ManufacturerSimpleModel> simplemodel = manufacturers.Select(manufacturer =>
            new ManufacturerSimpleModel
            {
                Id= manufacturer.Id,
                Name = manufacturer.Name,
            }).ToList();
                return simplemodel;
            });
        }

        private async Task<IList<VendorSimpleModel>> GetAllVendorsAsync()
        {
            InstantSearchComponent instantSearchComponent = this;
            IStaticCacheManager istaticCacheManager = instantSearchComponent.StaticCacheManager;
            CacheKey cacheKey = CacheKeys.NOP_INSTANT_SEARCH_VENDORS_MODEL_KEY;
            object obj1 = (object) ((BaseEntity) await instantSearchComponent._workContext.GetWorkingLanguageAsync()).Id;
            object obj2 = (object) await instantSearchComponent._aclHelper.GetAllowedCustomerRolesIdsAsync();
            Store currentStoreAsync = await instantSearchComponent._storeContext.GetCurrentStoreAsync();
            CacheKey cacheKey1 = istaticCacheManager.PrepareKeyForDefaultCache(cacheKey, new object[3]
            {
                obj1,
                obj2,
                (object) ((BaseEntity) currentStoreAsync).Id
            });
            istaticCacheManager = (IStaticCacheManager)null;
            cacheKey = (CacheKey)null;
            obj1 = (object)null;
            obj2 = (object)null;
            return await instantSearchComponent._staticCacheManager.GetAsync<IList<VendorSimpleModel>>(
                cacheKey1, async () =>
                {
                    IList<Vendor> vendors = _vendorRepository.Table.ToList();

                    IList<VendorSimpleModel> simplemodel = vendors.Select(vendor =>
                new VendorSimpleModel
                {
                    Id = vendor.Id,
                    Name = vendor.Name,
                }).ToList();
                return simplemodel;
                });
        }
    }
}
