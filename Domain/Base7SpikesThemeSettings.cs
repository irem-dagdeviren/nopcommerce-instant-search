using Nop.Core.Configuration;

namespace Nop.Plugin.InstantSearch.Domain
{
  public class Base7SpikesThemeSettings : ISettings
  {
    public bool ShowCartQuantityMarker { get; set; }

    public bool RemoveCopyright { get; set; }

    public bool RemoveDesignedBy { get; set; }

    public int LogoImageId { get; set; }

    public bool DoNotCacheLogoImageUrl { get; set; }

    public string PinterestUrl { get; set; }

    public string VimeoUrl { get; set; }

    public string InstagramUrl { get; set; }

    public string CustomHeadStyles { get; set; }

    public bool LazyLoadImages { get; set; }

    public Decimal ProductImageProportion { get; set; }

    public Decimal CategoryImageProportion { get; set; }

    public bool ProductBoxesImagesCarouselEnabled { get; set; }
  }
}
