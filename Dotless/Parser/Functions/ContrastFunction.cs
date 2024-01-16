// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Functions.ContrastFunction
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes;
using Nop.Plugin.InstantSearch.Dotless.Parser.Tree;
using Nop.Plugin.InstantSearch.Dotless.Utils;

namespace Nop.Plugin.InstantSearch.Dotless.Parser.Functions
{
  public class ContrastFunction : Function
  {
    protected override Node Evaluate(Env env)
    {
      Guard.ExpectMinArguments(1, this.Arguments.Count, (object) this, this.Location);
      Guard.ExpectMaxArguments(4, this.Arguments.Count, (object) this, this.Location);
      Guard.ExpectNode<Color>(this.Arguments[0], (object) this, this.Location);
      Color color1 = (Color) this.Arguments[0];
      if (this.Arguments.Count > 1)
        Guard.ExpectNode<Color>(this.Arguments[1], (object) this, this.Location);
      if (this.Arguments.Count > 2)
        Guard.ExpectNode<Color>(this.Arguments[2], (object) this, this.Location);
      if (this.Arguments.Count > 3)
        Guard.ExpectNode<Number>(this.Arguments[3], (object) this, this.Location);
      Color color2 = this.Arguments.Count > 1 ? (Color) this.Arguments[1] : new Color((double) byte.MaxValue, (double) byte.MaxValue, (double) byte.MaxValue);
      Color color3 = this.Arguments.Count > 2 ? (Color) this.Arguments[2] : new Color(0.0, 0.0, 0.0);
      double num = this.Arguments.Count > 3 ? ((Number) this.Arguments[3]).ToNumber() : 0.43;
      if (color3.Luma > color2.Luma)
      {
        Color color4 = color2;
        color2 = color3;
        color3 = color4;
      }
      return color1.Luma >= num ? (Node) color3 : (Node) color2;
    }
  }
}
