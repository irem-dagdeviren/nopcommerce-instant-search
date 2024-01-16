// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Functions.LightenFunction
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes;
using Nop.Plugin.InstantSearch.Dotless.Parser.Tree;
using Nop.Plugin.InstantSearch.Dotless.Utils;

namespace Nop.Plugin.InstantSearch.Dotless.Parser.Functions
{
  public class LightenFunction : HslColorFunctionBase
  {
    protected override Node EvalHsl(HslColor color) => (Node) color.GetLightness();

    protected override Node EditHsl(HslColor color, Number number)
    {
      color.Lightness += number.Value / 100.0;
      return (Node) color.ToRgbColor();
    }
  }
}
