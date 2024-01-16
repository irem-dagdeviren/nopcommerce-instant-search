// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Functions.AddFunction
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes;
using Nop.Plugin.InstantSearch.Dotless.Parser.Tree;
using Nop.Plugin.InstantSearch.Dotless.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nop.Plugin.InstantSearch.Dotless.Parser.Functions
{
  public class AddFunction : Function
  {
    protected override Node Evaluate(Env env)
    {
      Guard.ExpectAllNodes<Number>((IEnumerable<Node>) this.Arguments, (object) this, this.Location);
      return (Node) new Number(this.Arguments.Cast<Number>().Select<Number, double>((Func<Number, double>) (d => d.Value)).Aggregate<double, double>(0.0, (Func<double, double, double>) ((a, b) => a + b)));
    }
  }
}
