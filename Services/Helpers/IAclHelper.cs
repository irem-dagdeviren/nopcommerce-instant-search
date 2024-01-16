using Nop.Core.Domain.Catalog;

namespace Nop.Plugin.InstantSearch.Services.Helpers
{
  public interface IAclHelper
  {
        Task<IQueryable<Product>> GetAvailableProductsForCurrentCustomerAsync();

        Task<IQueryable<Manufacturer>> GetAvailableManufacturersForCurrentCustomerAsync();

        Task<IQueryable<Category>> GetAvailableCategoriesForCurrentCustomerAsync();

        Task<string> GetAllowedCustomerRolesIdsAsync();

        Task<Dictionary<int, int>> GetCategoryAndParentCategoryIdsForCurrentCustomerAsync();

    }
}
