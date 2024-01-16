using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Security;
using Nop.Data;
using Nop.Services.Customers;
using System.Linq.Expressions;
using System.Reflection;

#nullable enable
namespace Nop.Plugin.InstantSearch.Services.Helpers
{
  public class AclHelper : IAclHelper
  {
    private readonly 
    #nullable disable
    IWorkContext _workContext;
    private readonly IRepository<AclRecord> _aclRepository;
    private readonly IRepository<Product> _productRepository;
    private readonly IRepository<Manufacturer> _manufacturerRepository;
    private readonly IRepository<Category> _categoryRepository;
    private readonly CatalogSettings _catalogSettings;
    private readonly ICustomerService _customerService;

    public AclHelper(
      IWorkContext workContext,
      IRepository<AclRecord> aclRepository,
      IRepository<Product> productRepository,
      IRepository<Manufacturer> manufacturerRepository,
      IRepository<Category> categoryRepository,
      CatalogSettings catalogSettings,
      ICustomerService customerService)
    {
      this._workContext = workContext;
      this._aclRepository = aclRepository;
      this._productRepository = productRepository;
      this._manufacturerRepository = manufacturerRepository;
      this._categoryRepository = categoryRepository;
      this._catalogSettings = catalogSettings;
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
