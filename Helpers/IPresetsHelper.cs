
namespace Nop.Plugin.InstantSearch.Helpers
{
  public interface IPresetsHelper
  {
    string GeneratePresetCss(string commaSeparatedColors, string pluginSystemName);

    string GetLessContent(string pluginSystemName);

    string GenerateCssFromLess(string lessFileContent, string commaSeparatedColors);

    string GenerateImagesProportionsCss(
      string key,
      string value,
      string fileName,
      string pluginSystemName);
  }
}
