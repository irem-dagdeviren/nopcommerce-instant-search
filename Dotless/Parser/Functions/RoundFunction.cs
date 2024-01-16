// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Functions.RoundFunction
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes;
using Nop.Plugin.InstantSearch.Dotless.Parser.Tree;
using Nop.Plugin.InstantSearch.Dotless.Utils;
using System;

namespace Nop.Plugin.InstantSearch.Dotless.Parser.Functions
{
  public class RoundFunction : NumberFunctionBase
  {
    protected override Node Eval(Env env, Number number, Node[] args)
    {
      if (args.Length == 0)
        return (Node) new Number(Math.Round(number.Value, MidpointRounding.AwayFromZero), number.Unit);
      Guard.ExpectNode<Number>(args[0], (object) this, args[0].Location);
      return (Node) new Number(Math.Round(number.Value, (int) ((Number) args[0]).Value, MidpointRounding.AwayFromZero), number.Unit);
    }
  }
}
