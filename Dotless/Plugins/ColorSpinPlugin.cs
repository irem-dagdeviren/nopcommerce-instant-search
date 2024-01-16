// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Plugins.ColorSpinPlugin
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes;
using Nop.Plugin.InstantSearch.Dotless.Parser.Tree;
using Nop.Plugin.InstantSearch.Dotless.Utils;
using System.ComponentModel;

namespace Nop.Plugin.InstantSearch.Dotless.Plugins
{
  [Description("Automatically spins all colors in a less file")]
  [DisplayName("ColorSpin")]
  public class ColorSpinPlugin : VisitorPlugin
  {
    public double Spin { get; set; }

    public ColorSpinPlugin(double spin) => this.Spin = spin;

    public override VisitorPluginType AppliesTo => VisitorPluginType.AfterEvaluation;

    public override Node Execute(Node node, out bool visitDeeper)
    {
      visitDeeper = true;
      if (!(node is Color color))
        return node;
      HslColor hslColor = HslColor.FromRgbColor(color);
      hslColor.Hue += this.Spin / 360.0;
      return (Node) hslColor.ToRgbColor().ReducedFrom<Color>((Node) color);
    }
  }
}
