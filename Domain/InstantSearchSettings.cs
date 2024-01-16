using Nop.Core.Configuration;

namespace Nop.Plugin.InstantSearch.Domain
{
  public class InstantSearchSettings : ISettings
  {
    public bool Enable { get; set; }

    public int SearchOption { get; set; }

    public int NumberOfProducts { get; set; }

    public int NumberOfVisibleProducts { get; set; }

    public int DefaultProductSortOption { get; set; }

    public int PictureSize { get; set; }

    public bool ShowSku { get; set; }

    public bool HighlightFirstFoundElementToBeSelected { get; set; }

    public bool SearchDescriptions { get; set; }

    public bool SearchProductTags { get; set; }

    public bool VisibleIndividuallyOnly { get; set; }
  }
}
