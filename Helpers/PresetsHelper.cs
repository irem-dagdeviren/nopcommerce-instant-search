using Nop.Plugin.InstantSearch.Dotless;
using Nop.Core;
using Nop.Core.Infrastructure;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Nop.Plugin.InstantSearch.Helpers
{
  public class PresetsHelper : IPresetsHelper
  {
    private readonly IWebHelper _webHelper;
    private readonly INopFileProvider _nopFileProvider;

    public PresetsHelper(IWebHelper webHelper, INopFileProvider nopFileProvider)
    {
      this._webHelper = webHelper;
      this._nopFileProvider = nopFileProvider;
    }

    public string GeneratePresetCss(string commaSeparatedColors, string pluginSystemName)
    {
      string lessContent = this.GetLessContent(pluginSystemName);
      return !string.IsNullOrEmpty(lessContent) ? this.GenerateCssFromLess(lessContent, commaSeparatedColors) : string.Empty;
    }

    public string GetLessContent(string pluginSystemName)
    {
      string pathMappedToServer = this.GetFilePathMappedToServer(string.Format("~/Plugins/{0}/Styles/Less/preset.less", (object) pluginSystemName));
      return File.Exists(pathMappedToServer) ? File.ReadAllText(pathMappedToServer) : string.Empty;
    }

    public string GenerateCssFromLess(string lessFileContent, string commaSeparatedColors)
    {
      string empty = string.Empty;
      string[] strArray = commaSeparatedColors.Split(new char[1]
      {
        ','
      }, StringSplitOptions.RemoveEmptyEntries);
      bool flag = true;
      for (int index = 0; index < strArray.Length; ++index)
      {
        string oldValue = string.Format("%color-{0}%", (object) (index + 1));
        string color = strArray[index].Trim();
        if (!this.IsColorInHex(color))
        {
          flag = false;
          break;
        }
        string newValue = string.Format("#{0}", (object) color);
        lessFileContent = lessFileContent.Replace(oldValue, newValue);
      }
      if (flag)
        empty = Less.Parse(lessFileContent);
      return empty;
    }

    private string GetFilePathMappedToServer(string virtualFilePath) => this._nopFileProvider.MapPath(virtualFilePath);

    private bool IsColorInHex(string color) => Regex.IsMatch(color, "\\A\\b[0-9a-fA-F]+\\b\\Z");

    public string GenerateImagesProportionsCss(
      string key,
      string value,
      string fileName,
      string pluginSystemName)
    {
      string imagesProportions = this.GetLessContentForImagesProportions(fileName, pluginSystemName);
      return !string.IsNullOrEmpty(imagesProportions) ? this.GenerateCssForImagesFromLess(imagesProportions, key, value) : string.Empty;
    }

    public string GenerateCssForImagesFromLess(string lessFileContent, string key, string value)
    {
      lessFileContent = lessFileContent.Replace(string.Format("%{0}%", (object) key), value);
      return Less.Parse(lessFileContent);
    }

    private string GetLessContentForImagesProportions(string fileName, string pluginSystemName)
    {
      string pathMappedToServer = this.GetFilePathMappedToServer(string.Format("~/Plugins/{0}/Styles/Less/{1}", (object) pluginSystemName, (object) fileName));
      return File.Exists(pathMappedToServer) ? File.ReadAllText(pathMappedToServer) : string.Empty;
    }
  }
}
