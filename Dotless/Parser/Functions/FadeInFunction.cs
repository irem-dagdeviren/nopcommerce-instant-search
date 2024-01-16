// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Functions.FadeInFunction
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes;
using Nop.Plugin.InstantSearch.Dotless.Parser.Tree;

namespace Nop.Plugin.InstantSearch.Dotless.Parser.Functions
{
  public class FadeInFunction : ColorFunctionBase
  {
    protected override Node Eval(Color color) => (Node) new Number(color.Alpha);

    protected override Node EditColor(Color color, Number number)
    {
      double newAlpha = number.Value / 100.0;
      return (Node) new Color(color.R, color.G, color.B, this.ProcessAlpha(color.Alpha, newAlpha));
    }

    protected virtual double ProcessAlpha(double originalAlpha, double newAlpha) => originalAlpha + newAlpha;
  }
}
