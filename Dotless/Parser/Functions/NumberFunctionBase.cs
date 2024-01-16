// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Functions.NumberFunctionBase
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes;
using Nop.Plugin.InstantSearch.Dotless.Parser.Tree;
using Nop.Plugin.InstantSearch.Dotless.Utils;
using System.Linq;

namespace Nop.Plugin.InstantSearch.Dotless.Parser.Functions
{
  public abstract class NumberFunctionBase : Function
  {
    protected override Node Evaluate(Env env)
    {
      Guard.ExpectMinArguments(1, this.Arguments.Count, (object) this, this.Location);
      Guard.ExpectNode<Number>(this.Arguments[0], (object) this, this.Arguments[0].Location);
      Number number = this.Arguments[0] as Number;
      Node[] array = this.Arguments.Skip<Node>(1).ToArray<Node>();
      return this.Eval(env, number, array);
    }

    protected abstract Node Eval(Env env, Number number, Node[] args);
  }
}
