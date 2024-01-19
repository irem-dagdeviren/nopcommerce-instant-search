using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Services.Customers;

#nullable enable
namespace Nop.Plugin.InstantSearch.Services.Helpers
{
    public class AclHelper : IAclHelper
    {
        private readonly 
        #nullable disable
        IWorkContext _workContext;
        private readonly ICustomerService _customerService;

        public AclHelper(
            IWorkContext workContext,
            ICustomerService customerService)
        {
            this._workContext = workContext;
            this._customerService = customerService;
        }

        public async Task<string> GetAllowedCustomerRolesIdsAsync()
    {
        ICustomerService icustomerService = this._customerService;
        int[] customerRoleIdsAsync = await icustomerService.GetCustomerRoleIdsAsync(await this._workContext.GetCurrentCustomerAsync(), false);
        icustomerService = (ICustomerService)null;
        return string.Join<int>(",", (IEnumerable<int>)customerRoleIdsAsync);
    }

        public Task<IQueryable<Category>> GetAvailableCategoriesForCurrentCustomerAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<Manufacturer>> GetAvailableManufacturersForCurrentCustomerAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<Product>> GetAvailableProductsForCurrentCustomerAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<int, int>> GetCategoryAndParentCategoryIdsForCurrentCustomerAsync()
        {
            throw new NotImplementedException();
        }
    }
}
