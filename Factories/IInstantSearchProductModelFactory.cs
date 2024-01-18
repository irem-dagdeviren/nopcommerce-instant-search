using Nop.Web.Models.Catalog;
using Nop.Core.Domain.Catalog;

namespace Nop.Plugin.InstantSearch.Factories
{
    public partial interface IInstantSearchProductModelFactory
    {

        Task<IEnumerable<ProductOverviewModel>> PrepareProductOverviewModelsAsync(IEnumerable<Product> products,
            bool preparePriceModel = true, bool preparePictureModel = true,
            int? productThumbPictureSize = null, bool prepareSpecificationAttributes = false,
            bool forceRedirectionAfterAddingToCart = false);
        Task<ProductSpecificationModel> PrepareProductSpecificationModelAsync(Product product);
    }
}
