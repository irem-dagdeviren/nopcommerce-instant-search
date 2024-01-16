// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Functions.GreenFunction
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes;
using Nop.Plugin.InstantSearch.Dotless.Parser.Tree;

namespace Nop.Plugin.InstantSearch.Dotless.Parser.Functions
{
  public class GreenFunction : ColorFunctionBase
  {
    protected override Node Eval(Color color) => (Node) new Number(color.G);

    protected override Node EditColor(Color color, Number number)
    {
      this.WarnNotSupportedByLessJS("green(color, number)");
      double num = number.Value;
      if (number.Unit == "%")
        num = num * (double) byte.MaxValue / 100.0;
      return (Node) new Color(color.R, color.G + num, color.B);
    }
  }
}
