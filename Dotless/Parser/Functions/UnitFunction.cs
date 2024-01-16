// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Functions.UnitFunction
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes;
using Nop.Plugin.InstantSearch.Dotless.Parser.Tree;
using Nop.Plugin.InstantSearch.Dotless.Utils;

namespace Nop.Plugin.InstantSearch.Dotless.Parser.Functions
{
  public class UnitFunction : Function
  {
    protected override Node Evaluate(Env env)
    {
      Guard.ExpectMaxArguments(2, this.Arguments.Count, (object) this, this.Location);
      Guard.ExpectNode<Number>(this.Arguments[0], (object) this, this.Location);
      Number number = this.Arguments[0] as Number;
      string unit = string.Empty;
      if (this.Arguments.Count == 2)
        unit = !(this.Arguments[1] is Keyword) ? this.Arguments[1].ToCSS(env) : ((Keyword) this.Arguments[1]).Value;
      return (Node) new Number(number.Value, unit);
    }
  }
}
