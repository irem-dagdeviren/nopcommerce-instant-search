using Nop.Core;

namespace Nop.Plugin.InstantSearch.Domain
{
  public class NopTemplatesThemeConfiguration : BaseEntity
  {
    public string CustomHeadStyles { get; set; }

    public string PresetCss { get; set; }

    public string ProductImageProportionCss { get; set; }

    public string CategoryImageProportionCss { get; set; }
  }
}
