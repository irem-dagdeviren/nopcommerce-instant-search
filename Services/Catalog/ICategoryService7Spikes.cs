using Nop.Core.Domain.Catalog;

namespace Nop.Plugin.InstantSearch.Services.Catalog
{
  public interface ICategoryService7Spikes
  {
    Task<IList<Category>> GetCategoriesByParentCategoryIdAsync(
      int parentCategoryId,
      bool includeSubCategoriesFromAllLevels = false,
      bool showHidden = false);

    Task<List<int>> GetCategoryIdsByParentCategoryAsync(int categoryId, bool showHidden = false);

    Task<IList<Category>> GetAvailableCategoriesAsync(bool showHidden = false);
  }
}
