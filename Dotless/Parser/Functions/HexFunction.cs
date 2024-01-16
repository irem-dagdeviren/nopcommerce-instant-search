// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Functions.HexFunction
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Exceptions;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes;
using Nop.Plugin.InstantSearch.Dotless.Parser.Tree;

namespace Nop.Plugin.InstantSearch.Dotless.Parser.Functions
{
  public class HexFunction : NumberFunctionBase
  {
    protected override Node Eval(Env env, Number number, Node[] args)
    {
      this.WarnNotSupportedByLessJS("hex(number)");
      number.Value = string.IsNullOrEmpty(number.Unit) ? HexFunction.Clamp(number.Value, (double) byte.MaxValue, 0.0) : throw new ParsingException(string.Format("Expected unitless number in function 'hex', found {0}", (object) number.ToCSS(env)), number.Location);
      return (Node) new TextNode(((int) number.Value).ToString("X2"));
    }

    private static double Clamp(double value, double max, double min)
    {
      if (value < min)
        value = min;
      if (value > max)
        value = max;
      return value;
    }
  }
}
