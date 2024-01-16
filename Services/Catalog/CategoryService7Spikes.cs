using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Security;
using Nop.Core.Domain.Stores;
using Nop.Data;
using Nop.Services.Catalog;
using Nop.Services.Customers;
using Nop.Plugin.InstantSearch.Services.Helpers;


#nullable enable
namespace Nop.Plugin.InstantSearch.Services.Catalog
{
  public class CategoryService7Spikes : ICategoryService7Spikes
  {
    private readonly 
    #nullable disable
    CatalogSettings _catalogSettings;
    private readonly IStoreContext _storeContext;
    private readonly IRepository<Category> _categoryRepository;
    private readonly IRepository<AclRecord> _aclRepository;
    private readonly IRepository<StoreMapping> _storeMappingRepository;
    private readonly ICategoryService _categoryService;
    private readonly ICustomerService _customerService;
    private readonly IStaticCacheManager _staticCacheManager;
    private readonly IWorkContext _workContext;

    public static CacheKey CATEGORIES_WITH_CHILDREN_KEY => new CacheKey("Nop.categories.with.children.available.categories.store.id-{0}-{1}-{2}", Array.Empty<string>());

    public CategoryService7Spikes(
      IStoreContext storeContext,
      IRepository<Category> categoryRepository,
      IWorkContext workContext,
      CatalogSettings catalogSettings,
      IRepository<StoreMapping> storeMappingRepository,
      IRepository<AclRecord> aclRepository,
      ICustomerService customerService,
      ICategoryService categoryService,
      IStaticCacheManager staticCacheManager)
    {
      this._storeContext = storeContext;
      this._categoryRepository = categoryRepository;
      this._workContext = workContext;
      this._catalogSettings = catalogSettings;
      this._storeMappingRepository = storeMappingRepository;
      this._aclRepository = aclRepository;
      this._customerService = customerService;
      this._categoryService = categoryService;
      this._staticCacheManager = staticCacheManager;
    }

    public async Task<List<int>> GetCategoryIdsByParentCategoryAsync(
      int categoryId,
      bool showHidden = false)
    {
      return (await this.GetCategoriesWithChildrenTree(showHidden)).GetAllSubNodes(categoryId).Select<TreeNode<int>, int>((Func<TreeNode<int>, int>) (x => x.Value)).ToList<int>();
    }

    public async Task<IList<Category>> GetCategoriesByParentCategoryIdAsync(
      int parentCategoryId,
      bool includeSubCategoriesFromAllLevels = false,
      bool showHidden = false)
    {
      return this.GetCategoriesByParentCategoryIdInternal(await this.GetAvailableCategoriesAsync(showHidden), parentCategoryId, includeSubCategoriesFromAllLevels, showHidden);
    }

    public async Task<IList<Category>> GetAvailableCategoriesAsync(bool showHidden = false) => await this._categoryService.GetAllCategoriesAsync(((BaseEntity) await this._storeContext.GetCurrentStoreAsync()).Id, showHidden);

    private async Task<Tree<int>> GetCategoriesWithChildrenTree(bool showHidden = false)
    {
      ICustomerService icustomerService = this._customerService;
      int[] customerRoleIdsAsync = await icustomerService.GetCustomerRoleIdsAsync(await this._workContext.GetCurrentCustomerAsync(), false);
      icustomerService = (ICustomerService) null;
      int[] customerRolesIds = customerRoleIdsAsync;
      IStaticCacheManager istaticCacheManager = this._staticCacheManager;
      CacheKey cacheKey = CategoryService7Spikes.CATEGORIES_WITH_CHILDREN_KEY;
      Store currentStoreAsync = await this._storeContext.GetCurrentStoreAsync();
      CacheKey cacheKey1 = istaticCacheManager.PrepareKeyForDefaultCache(cacheKey, new object[3]
      {
        (object) ((BaseEntity) currentStoreAsync).Id,
        (object) showHidden,
        (object) customerRolesIds
      });
      istaticCacheManager = (IStaticCacheManager) null;
      cacheKey = (CacheKey) null;
      Tree<int> categoryTree = new Tree<int>(0);
      foreach (KeyValuePair<int, int> keyValuePair in await this._staticCacheManager.GetAsync<Dictionary<int, int>>(cacheKey1, (Func<Task<Dictionary<int, int>>>) (() => this.GetAvailableCategoriesWithParentDictionary(showHidden))))
      {
        int parentValue = keyValuePair.Value;
        categoryTree.Add(keyValuePair.Key, parentValue);
      }
      Tree<int> withChildrenTree = categoryTree;
      customerRolesIds = (int[]) null;
      categoryTree = (Tree<int>) null;
      return withChildrenTree;
    }

    private async Task<Dictionary<int, int>> GetAvailableCategoriesWithParentDictionary(
      bool showHidden)
    {
      var list = (await this.GetAvailableCategoriesAsync(showHidden)).Select(c => new  //
      {
        categoryId = ((BaseEntity) c).Id,
        parentCategoryId = c.ParentCategoryId
      }).ToList();
      Dictionary<int, int> parentDictionary = new Dictionary<int, int>();
      foreach (var data in list)
      {
        var categoryWithParent = data;
        if (!parentDictionary.ContainsKey(categoryWithParent.categoryId) && (categoryWithParent.parentCategoryId == 0 || list.Any(c => c.categoryId == categoryWithParent.parentCategoryId)))
          parentDictionary.Add(categoryWithParent.categoryId, categoryWithParent.parentCategoryId);
      }
      return parentDictionary;
    }

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
