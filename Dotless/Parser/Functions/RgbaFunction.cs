// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Functions.RgbaFunction
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
  public class RgbaFunction : Function
  {
    protected override Node Evaluate(Env env)
    {
      if (this.Arguments.Count == 2)
        return (Node) new Color(Guard.ExpectNode<Color>(this.Arguments[0], (object) this, this.Location).RGB, Guard.ExpectNode<Number>(this.Arguments[1], (object) this, this.Location).Value);
      Guard.ExpectNumArguments(4, this.Arguments.Count, (object) this, this.Location);
      List<Number> source = Guard.ExpectAllNodes<Number>((IEnumerable<Node>) this.Arguments, (object) this, this.Location);
      return (Node) new Color(source.Take<Number>(3).Select<Number, double>((Func<Number, double>) (n => n.ToNumber((double) byte.MaxValue))).ToArray<double>(), source[3].ToNumber(1.0));
    }
  }
}
