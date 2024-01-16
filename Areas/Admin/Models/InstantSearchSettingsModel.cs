using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.InstantSearch.Areas.Admin.Models
{
  public class InstantSearchSettingsModel
  {
    public InstantSearchSettingsModel()
    {
      this.AvailableProductSortOptions = (IList<SelectListItem>) new List<SelectListItem>();
      this.AvailableSearchOptions = (IList<SelectListItem>) new List<SelectListItem>();
    }

    public bool IsTrialVersion { get; set; }

    [NopResourceDisplayName("InstantSearch.Admin.Settings.SearchOption")]
    public int SearchOption { get; set; }

    public bool SearchOption_OverrideForStore { get; set; }

    public IList<SelectListItem> AvailableSearchOptions { get; set; }

    [NopResourceDisplayName("InstantSearch.Admin.Settings.Enable")]
    public bool Enable { get; set; }

    public bool Enable_OverrideForStore { get; set; }

    [NopResourceDisplayName("InstantSearch.Admin.Settings.NumberOfProducts")]
    public int NumberOfProducts { get; set; }

    public bool NumberOfProducts_OverrideForStore { get; set; }

    [NopResourceDisplayName("InstantSearch.Admin.Settings.NumberOfVisibleProducts")]
    public int NumberOfVisibleProducts { get; set; }

    public bool NumberOfVisibleProducts_OverrideForStore { get; set; }

    [NopResourceDisplayName("InstantSearch.Admin.Settings.DefaultProductSortOption")]
    public int DefaultProductSortOption { get; set; }

    public bool DefaultProductSortOption_OverrideForStore { get; set; }

    public IList<SelectListItem> AvailableProductSortOptions { get; set; }

    [NopResourceDisplayName("InstantSearch.Admin.Settings.PictureSizeInPixels")]
    public int PictureSize { get; set; }

    public bool PictureSize_OverrideForStore { get; set; }

    [NopResourceDisplayName("InstantSearch.Admin.Settings.SearchDescriptions")]
    public bool SearchDescriptions { get; set; }

    public bool SearchDescriptions_OverrideForStore { get; set; }

    [NopResourceDisplayName("InstantSearch.Admin.Settings.SearchProductTags")]
    public bool SearchProductTags { get; set; }

    public bool SearchProductTags_OverrideForStore { get; set; }

    [NopResourceDisplayName("InstantSearch.Admin.Settings.ShowSku")]
    public bool ShowSku { get; set; }

    public bool ShowSku_OverrideForStore { get; set; }

    [NopResourceDisplayName("InstantSearch.Admin.Settings.HighlightFirstFoundElementToBeSelected")]
    public bool HighlightFirstFoundElementToBeSelected { get; set; }

    public bool HighlightFirstFoundElementToBeSelected_OverrideForStore { get; set; }

    [NopResourceDisplayName("InstantSearch.Admin.Settings.MinKeywordLength")]
    public int MinKeywordLength { get; set; }

    public bool MinKeywordLength_OverrideForStore { get; set; }

    [NopResourceDisplayName("InstantSearch.Admin.Settings.VisibleIndividuallyOnly")]
    public bool VisibleIndividuallyOnly { get; set; }

    public bool VisibleIndividuallyOnly_OverrideForStore { get; set; }

    [NopResourceDisplayName("InstantSearch.Admin.Settings.AutocompleteHeight")]
    public int AutocompleteHeight { get; set; }

    public bool AutocompleteHeight_OverrideForStore { get; set; }

    public string Theme { get; set; }

    public int ActiveStoreScopeConfiguration { get; set; }
  }
}
