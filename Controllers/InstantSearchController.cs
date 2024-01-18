using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Plugin.InstantSearch.Domain;
using Nop.Plugin.InstantSearch.Factories;
using Nop.Plugin.InstantSearch.Models;
using Nop.Plugin.InstantSearch.Services.Catalog;
using Nop.Services.Catalog;
using Nop.Services.Configuration;
using Nop.Services.Customers;
using Nop.Web.Framework.Models;
using Nop.Web.Models.Catalog;
using UrlHelperExtensions = Nop.Web.Framework.Mvc.Routing.UrlHelperExtensions;

namespace Nop.Plugin.InstantSearch.Controllers
{
    public class InstantSearchController : Base7SpikesPublicController
    {
        private readonly IProductService _productService;
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly ICategoryService7Spikes _categoryServiceSevenSpikes;
        private readonly ISettingService _settingService;
        private readonly IInstantSearchProductModelFactory _instantSearchProductModelFactory;
        private readonly DuzeySearchSettings _instantSearchSettings;
        private readonly ICustomerService _customerService;


        public InstantSearchController(
                IProductService productService,
              IWorkContext workContext,
              IStoreContext storeContext,
              ICategoryService7Spikes categoryServiceSevenSpikes,
              ISettingService settingService,
              IInstantSearchProductModelFactory instantSearchProductModelFactory,
              DuzeySearchSettings instantSearchSettings,
              ICustomerService customerService
      )
        {
            _productService = productService;
            _storeContext = storeContext;
            _categoryServiceSevenSpikes = categoryServiceSevenSpikes;
            _workContext = workContext;
            _settingService = settingService;
            _instantSearchSettings = instantSearchSettings;
            _customerService = customerService;
            _instantSearchProductModelFactory = instantSearchProductModelFactory;
        }

        public async Task<ActionResult> InstantSearchForAction(
          string q,
          int categoryId = 0,
          int manufacturerId = 0,
          int vendorId = 0)
        {
            InstantSearchController searchController = this;
            InstantSearchDropdownModel model = await searchController.SearchProductsAsync(q, categoryId, manufacturerId, vendorId);
            InstantSearchDropdownModel searchDropdownModel = model;
            searchDropdownModel.ShowAllButtonHtml = await ((InstantSearchController)searchController).RenderPartialViewToStringAsync("~/Plugins/InstantSearch/Views/InstantSearch/_ShowAllResults.cshtml", (object)model.TotalProducts);
            searchDropdownModel = (InstantSearchDropdownModel)null;
            ActionResult actionResult = (ActionResult)((Controller)searchController).Json((object)model);
            model = (InstantSearchDropdownModel)null;
            return actionResult;
        }

        private async Task<InstantSearchDropdownModel> SearchProductsAsync(
          string q,
          int categoryId,
          int manufacturerId,
          int vendorId)
        {
            InstantSearchController searchController = this;
            IPagedList<Product> products;
            if (manufacturerId != 0)
                products = await searchController.GetProductsByManufacturerIdAsync(q, manufacturerId);
            else if (vendorId != 0)
                products = await searchController.GetProductsByVendorIdAsync(q, vendorId);
            else if (categoryId != 0)
                products = await searchController.GetProductsByCategoryIdAsync(q, categoryId);
            else
                products = await searchController.GetProductsAsync(q);

            ISettingService isettingService = searchController._settingService;
            int num = await isettingService.GetSettingByKeyAsync<bool>("duzeysearchsettings.preparespecificationattributes", false, ((BaseEntity)await searchController._storeContext.GetCurrentStoreAsync()).Id, true) ? 1 : 0;
            isettingService = (ISettingService)null;
            bool flag = num != 0;
            IEnumerable<ProductOverviewModel> productOverviewModels = await searchController._instantSearchProductModelFactory.PrepareProductOverviewModelsAsync((IEnumerable<Product>)products, true, true, new int?(searchController._instantSearchSettings.PictureSize), flag, false);

            foreach (Product product1 in (IEnumerable<Product>)products)
            {
                Product product = product1;
                ProductOverviewModel currentProductModel = productOverviewModels.FirstOrDefault<ProductOverviewModel>((Func<ProductOverviewModel, bool>)(x => ((BaseNopEntityModel)x).Id == ((BaseEntity)product).Id));

                if (!((BaseNopModel)currentProductModel).CustomProperties.ContainsKey("Url"))
                {
                    string seNameAsync = await searchController.UrlRecordService.GetSeNameAsync<Product>(product, new int?(), true, true);
                    ((BaseNopModel)currentProductModel).CustomProperties.Add("Url", UrlHelperExtensions.RouteUrl<Product>(((ControllerBase)searchController).Url, (object)new
                    {
                        SeName = seNameAsync
                    }, (string)null, (string)null, (string)null));
                }

                currentProductModel = (ProductOverviewModel)null;
            }
            if (searchController._instantSearchSettings.ShowSku)
            {
                foreach (Product product2 in (IEnumerable<Product>)products)
                {

                    Product product = product2;
                    ProductOverviewModel productOverviewModel = productOverviewModels.FirstOrDefault<ProductOverviewModel>((Func<ProductOverviewModel, bool>)(x => ((BaseNopEntityModel)x).Id == ((BaseEntity)product).Id));


                    if (!productOverviewModel.CustomProperties.ContainsKey("Sku"))
                    {
                        Dictionary<string, string> dictionary = ((BaseNopModel)productOverviewModel).CustomProperties;
                        dictionary.Add("Sku", await searchController._productService.FormatSkuAsync(product, (string)null));
                        dictionary = (Dictionary<string, string>)null;
                    }
                }
            }
            InstantSearchDropdownModel searchDropdownModel = new InstantSearchDropdownModel()
            {
                Products = (IList<ProductOverviewModel>)productOverviewModels.ToList<ProductOverviewModel>(),
                TotalProducts = products.TotalCount
            };
            products = (IPagedList<Product>)null;
            productOverviewModels = (IEnumerable<ProductOverviewModel>)null;
            return searchDropdownModel;
        }

        private async Task<IPagedList<Product>> GetProductsByManufacturerIdAsync(
          string q,
          int manufacturerId)
        {
            IProductService iproductService = this._productService;
            int numberOfProducts = this._instantSearchSettings.NumberOfProducts;
            int id = ((BaseEntity)await this._storeContext.GetCurrentStoreAsync()).Id;
            string str = q;
            int id1 = ((BaseEntity)await this._workContext.GetWorkingLanguageAsync()).Id;
            bool searchDescriptions = this._instantSearchSettings.SearchDescriptions;
            IProductService iproductService1 = iproductService;
            int num1 = numberOfProducts;
            List<int> intList = new List<int>();
            intList.Add(manufacturerId);
            int num2 = id;
            bool searchProductTags = this._instantSearchSettings.SearchProductTags;
            bool individuallyOnly = this._instantSearchSettings.VisibleIndividuallyOnly;
            ProductSortingEnum productSortOption = (ProductSortingEnum)this._instantSearchSettings.DefaultProductSortOption;
            ProductType? nullable1 = new ProductType?();
            int num3 = individuallyOnly ? 1 : 0;
            Decimal? nullable2 = new Decimal?();
            Decimal? nullable3 = new Decimal?();
            string str1 = str;
            int num4 = searchDescriptions ? 1 : 0;
            int num5 = searchProductTags ? 1 : 0;
            int num6 = id1;
            ProductSortingEnum productSortingEnum = productSortOption;
            bool? nullable4 = new bool?();
            IPagedList<Product> manufacturerIdAsync = await iproductService1.SearchProductsAsync(0, num1, (IList<int>)null, (IList<int>)intList, num2, 0, 0, nullable1, num3 != 0, false, nullable2, nullable3, 0, str1, num4 != 0, true, true, num5 != 0, num6, (IList<SpecificationAttributeOption>)null, productSortingEnum, false, nullable4);
            str = (string)null;
            iproductService = (IProductService)null;
            return manufacturerIdAsync;
        }

        private async Task<IPagedList<Product>> GetProductsByVendorIdAsync(string q, int vendorId)
        {
            IProductService iproductService = this._productService;
            int numberOfProducts = this._instantSearchSettings.NumberOfProducts;
            int id = ((BaseEntity)await this._storeContext.GetCurrentStoreAsync()).Id;
            string str = q;
            int id1 = ((BaseEntity)await this._workContext.GetWorkingLanguageAsync()).Id;
            bool searchDescriptions = this._instantSearchSettings.SearchDescriptions;
            IProductService iproductService1 = iproductService;
            int num1 = numberOfProducts;
            int num2 = id;
            int num3 = vendorId;
            bool searchProductTags = this._instantSearchSettings.SearchProductTags;
            bool individuallyOnly = this._instantSearchSettings.VisibleIndividuallyOnly;
            ProductSortingEnum productSortOption = (ProductSortingEnum)this._instantSearchSettings.DefaultProductSortOption;
            ProductType? nullable1 = new ProductType?();
            int num4 = individuallyOnly ? 1 : 0;
            Decimal? nullable2 = new Decimal?();
            Decimal? nullable3 = new Decimal?();
            string str1 = str;
            int num5 = searchDescriptions ? 1 : 0;
            int num6 = searchProductTags ? 1 : 0;
            int num7 = id1;
            ProductSortingEnum productSortingEnum = productSortOption;
            bool? nullable4 = new bool?();
            IPagedList<Product> productsByVendorIdAsync = await iproductService1.SearchProductsAsync(0, num1, (IList<int>)null, (IList<int>)null, num2, num3, 0, nullable1, num4 != 0, false, nullable2, nullable3, 0, str1, num5 != 0, true, true, num6 != 0, num7, (IList<SpecificationAttributeOption>)null, productSortingEnum, false, nullable4);
            str = (string)null;
            iproductService = (IProductService)null;
            return productsByVendorIdAsync;
        }

        private async Task<IPagedList<Product>> GetProductsByCategoryIdAsync(string q, int categoryId)
        {
            IList<int> list = (IList<int>)(await this._categoryServiceSevenSpikes.GetCategoriesByParentCategoryIdAsync(categoryId, true, false)).Select<Category, int>((Func<Category, int>)(c => ((BaseEntity)c).Id)).ToList<int>();
            list.Add(categoryId);
            IProductService iproductService = this._productService;
            int numberOfProducts = this._instantSearchSettings.NumberOfProducts;
            IList<int> intList = list;
            int id = ((BaseEntity)await this._storeContext.GetCurrentStoreAsync()).Id;
            string str = q;
            int id1 = ((BaseEntity)await this._workContext.GetWorkingLanguageAsync()).Id;
            bool searchDescriptions = this._instantSearchSettings.SearchDescriptions;
            bool searchProductTags = this._instantSearchSettings.SearchProductTags;
            bool individuallyOnly = this._instantSearchSettings.VisibleIndividuallyOnly;
            ProductSortingEnum productSortOption = (ProductSortingEnum)this._instantSearchSettings.DefaultProductSortOption;
            IPagedList<Product> byCategoryIdAsync = await iproductService.SearchProductsAsync(0, numberOfProducts, intList, (IList<int>)null, id, 0, 0, new ProductType?(), individuallyOnly, false, new Decimal?(), new Decimal?(), 0, str, searchDescriptions, true, true, searchProductTags, id1, (IList<SpecificationAttributeOption>)null, productSortOption, false, new bool?());
            str = (string)null;
            iproductService = (IProductService)null;
            intList = (IList<int>)null;
            return byCategoryIdAsync;
        }

        private async Task<IPagedList<Product>> GetProductsAsync(string q)
        {
            ICustomerService icustomerservice = this._customerService;
            IProductService iproductService = this._productService;
            int numberOfProducts = this._instantSearchSettings.NumberOfProducts;
            int id = ((BaseEntity)await this._storeContext.GetCurrentStoreAsync()).Id;
            string str = q;
            int id1 = ((BaseEntity)await this._workContext.GetWorkingLanguageAsync()).Id;
            bool searchDescriptions = this._instantSearchSettings.SearchDescriptions;
            bool searchProductTags = this._instantSearchSettings.SearchProductTags;
            bool individuallyOnly = this._instantSearchSettings.VisibleIndividuallyOnly;
            ProductSortingEnum productSortOption = (ProductSortingEnum)this._instantSearchSettings.DefaultProductSortOption;
            IPagedList<Product> productsAsync = await iproductService.SearchProductsAsync(0, numberOfProducts, (IList<int>)null, (IList<int>)null, id, 0, 0, new ProductType?(), individuallyOnly, false, new Decimal?(), new Decimal?(), 0, str, searchDescriptions, true, true, searchProductTags, id1, (IList<SpecificationAttributeOption>)null, productSortOption, false, new bool?());

            str = (string)null;
            iproductService = (IProductService)null;


            return productsAsync;
        }
    }
}

