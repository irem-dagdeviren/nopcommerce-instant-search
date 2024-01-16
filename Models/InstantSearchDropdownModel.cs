using Nop.Web.Models.Catalog;

namespace Nop.Plugin.InstantSearch.Models
{
  public class InstantSearchDropdownModel
  {
    public IList<ProductOverviewModel> Products { get; set; }

    public int TotalProducts { get; set; }

    public string ShowAllButtonHtml { get; set; }
  }
}
