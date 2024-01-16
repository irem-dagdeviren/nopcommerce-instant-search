// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Expression
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes;
using Nop.Plugin.InstantSearch.Dotless.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nop.Plugin.InstantSearch.Dotless.Parser.Tree
{
  public class Expression : Node
  {
    public NodeList Value { get; set; }

    private bool IsExpressionList { get; set; }

    public Expression(IEnumerable<Node> value)
      : this(value, false)
    {
    }

    public Expression(IEnumerable<Node> value, bool isExpressionList)
    {
      this.IsExpressionList = isExpressionList;
      if (value is NodeList)
        this.Value = value as NodeList;
      else
        this.Value = new NodeList(value);
    }

    public override Node Evaluate(Env env)
    {
      if (this.Value.Count > 1)
        return new Expression((IEnumerable<Node>) new NodeList(this.Value.Select<Node, Node>((Func<Node, Node>) (e => e.Evaluate(env)))), this.IsExpressionList).ReducedFrom<Node>((Node) this);
      if (this.Value.Count != 1)
        return (Node) this;
      return this.Value[0].Evaluate(env).ReducedFrom<Node>((Node) this);
    }

    protected override Node CloneCore() => (Node) new Expression((IEnumerable<Node>) this.Value.Clone(), this.IsExpressionList);

    public override void AppendCSS(Env env) => env.Output.AppendMany<Node>((IEnumerable<Node>) this.Value, this.IsExpressionList ? ", " : " ");

    public override void Accept(IVisitor visitor) => this.Value = this.VisitAndReplace<NodeList>(this.Value, visitor);
  }
}
