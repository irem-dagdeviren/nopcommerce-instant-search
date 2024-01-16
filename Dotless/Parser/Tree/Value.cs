// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Value
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
  public class Value : Node
  {
    public NodeList Values { get; set; }

    public NodeList PreImportantComments { get; set; }

    public string Important { get; set; }

    public Value(IEnumerable<Node> values, string important)
    {
      this.Values = new NodeList(values);
      this.Important = important;
    }

    protected override Node CloneCore() => (Node) new Value((IEnumerable<Node>) this.Values.Clone(), this.Important);

    public override void AppendCSS(Env env)
    {
      env.Output.AppendMany<Node>((IEnumerable<Node>) this.Values, env.Compress ? "," : ", ");
      if (string.IsNullOrEmpty(this.Important))
        return;
      if ((bool) (Node) this.PreImportantComments)
        env.Output.Append((Node) this.PreImportantComments);
      env.Output.Append(" ").Append(this.Important);
    }

    public override string ToString() => this.ToCSS(new Env((Nop.Plugin.InstantSearch.Dotless.Parser.Parser) null));

    public override Node Evaluate(Env env)
    {
      Node node;
      if (this.Values.Count == 1 && string.IsNullOrEmpty(this.Important))
        node = this.Values[0].Evaluate(env);
      else
        ((Value) (node = (Node) new Value(this.Values.Select<Node, Node>((Func<Node, Node>) (n => n.Evaluate(env))), this.Important))).PreImportantComments = this.PreImportantComments;
      return node.ReducedFrom<Node>((Node) this);
    }

    public override void Accept(IVisitor visitor) => this.Values = this.VisitAndReplace<NodeList>(this.Values, visitor);
  }
}
