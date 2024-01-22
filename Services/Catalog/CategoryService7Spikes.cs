using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Domain.Catalog;
using Nop.Services.Catalog;
using Nop.Services.Customers;


#nullable enable
namespace Nop.Plugin.InstantSearch.Services.Catalog
{
    public class CategoryService7Spikes : ICategoryService7Spikes
    {
        private readonly IStoreContext _storeContext;
        private readonly ICategoryService _categoryService;

        public CategoryService7Spikes(
            IStoreContext storeContext,
            IWorkContext workContext,
            ICustomerService customerService,
            ICategoryService categoryService,
            IStaticCacheManager staticCacheManager)
        {
            this._storeContext = storeContext;
            this._categoryService = categoryService;
        }

        public async Task<IList<Category>> GetCategoriesByParentCategoryIdAsync(
            int parentCategoryId,
            bool includeSubCategoriesFromAllLevels = false,
            bool showHidden = false)
        {
            return this.GetCategoriesByParentCategoryIdInternal(await this.GetAvailableCategoriesAsync(showHidden), parentCategoryId, includeSubCategoriesFromAllLevels, showHidden);
        }

        public async Task<IList<Category>> GetAvailableCategoriesAsync(bool showHidden = false) => await this._categoryService.GetAllCategoriesAsync(((BaseEntity) await this._storeContext.GetCurrentStoreAsync()).Id, showHidden);

        private IList<Category> GetCategoriesByParentCategoryIdInternal(
            IList<Category> availableCategories,
            int parentCategoryId,
            bool includeSubCategoriesFromAllLevels,
            bool showHidden)
        {
            List<Category> list = availableCategories.Where<Category>((Func<Category, bool>) (c => c.ParentCategoryId == parentCategoryId)).ToList<Category>();
            List<Category> categoryIdInternal1 = new List<Category>((IEnumerable<Category>) list);
            if (includeSubCategoriesFromAllLevels)
            {
                foreach (Category category in list)
                {
                    IList<Category> categoryIdInternal2 = this.GetCategoriesByParentCategoryIdInternal(availableCategories, ((BaseEntity) category).Id, true, showHidden);
                    categoryIdInternal1.AddRange((IEnumerable<Category>) categoryIdInternal2);
                }
            }
            return (IList<Category>) categoryIdInternal1;
        }
    }
}
