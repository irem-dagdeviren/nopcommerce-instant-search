using Nop.Core.Domain.Catalog;

namespace Nop.Plugin.InstantSearch.Services.Helpers
{
    public interface IAclHelper
    {
        Task<string> GetAllowedCustomerRolesIdsAsync();
    }
}
