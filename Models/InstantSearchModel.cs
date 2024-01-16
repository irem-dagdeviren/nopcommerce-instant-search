using Nop.Web.Models.Catalog;

namespace Nop.Plugin.InstantSearch.Models
{
  public class InstantSearchModel
  {
    public InstantSearchModel()
    {
      this.TopLevelCategories = (IList<CategorySimpleModel>) new List<CategorySimpleModel>();
      this.Manufacturers = (IList<ManufacturerSimpleModel>) new List<ManufacturerSimpleModel>();
      this.Vendors = (IList<VendorSimpleModel>) new List<VendorSimpleModel>();
    }

    public string Theme { get; set; }

    public bool SearchInProductDescriptions { get; set; }

    public int MinKeywordLength { get; set; }

    public int DefaultProductSortOption { get; set; }

    public bool HighlightFirstFoundElementToBeSelected { get; set; }

    public bool ShowSku { get; set; }

    public int NumberOfVisibleProducts { get; set; }

    public int MaximumNumberOfProducts { get; set; }

    public IList<CategorySimpleModel> TopLevelCategories { get; set; }

    public IList<ManufacturerSimpleModel> Manufacturers { get; set; }

    public IList<VendorSimpleModel> Vendors { get; set; }
  }
}
