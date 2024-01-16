// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Extend
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nop.Plugin.InstantSearch.Dotless.Parser.Tree
{
  public class Extend : Node
  {
    public Extend(List<Selector> exact, List<Selector> partial)
    {
      this.Exact = exact;
      this.Partial = partial;
    }

    public List<Selector> Exact { get; set; }

    public List<Selector> Partial { get; set; }

    public override Node Evaluate(Env env)
    {
      List<Selector> exact = new List<Selector>();
      foreach (Selector selector1 in this.Exact)
      {
        Env childEnv = env.CreateChildEnv();
        selector1.AppendCSS(childEnv);
        Selector selector2 = new Selector((IEnumerable<Element>) new Element[1]
        {
          new Element(selector1.Elements.First<Element>().Combinator, childEnv.Output.ToString().Trim())
        });
        selector2.IsReference = this.IsReference;
        exact.Add(selector2);
      }
      List<Selector> partial = new List<Selector>();
      foreach (Selector selector3 in this.Partial)
      {
        Env childEnv = env.CreateChildEnv();
        selector3.AppendCSS(childEnv);
        Selector selector4 = new Selector((IEnumerable<Element>) new Element[1]
        {
          new Element(selector3.Elements.First<Element>().Combinator, childEnv.Output.ToString().Trim())
        });
        selector4.IsReference = this.IsReference;
        partial.Add(selector4);
      }
      Extend extend = new Extend(exact, partial);
      extend.IsReference = this.IsReference;
      extend.Location = this.Location;
      return (Node) extend;
    }

    protected override Node CloneCore() => (Node) new Extend(this.Exact.Select<Selector, Selector>((Func<Selector, Selector>) (e => (Selector) e.Clone())).ToList<Selector>(), this.Partial.Select<Selector, Selector>((Func<Selector, Selector>) (e => (Selector) e.Clone())).ToList<Selector>());

    public override void AppendCSS(Env env)
    {
    }

    public override bool IgnoreOutput() => true;
  }
}
