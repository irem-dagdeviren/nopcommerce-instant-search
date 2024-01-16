// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Functions.PowFunction
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
  public class PowFunction : Function
  {
    protected override Node Evaluate(Env env)
    {
      Guard.ExpectMinArguments(2, this.Arguments.Count, (object) this, this.Location);
      Guard.ExpectMaxArguments(2, this.Arguments.Count, (object) this, this.Location);
      Guard.ExpectAllNodes<Number>((IEnumerable<Node>) this.Arguments, (object) this, this.Location);
      this.WarnNotSupportedByLessJS("pow(number, number)");
      return (Node) new Number(Math.Pow(this.Arguments.Cast<Number>().First<Number>().Value, this.Arguments.Cast<Number>().ElementAt<Number>(1).Value));
    }
  }
}
