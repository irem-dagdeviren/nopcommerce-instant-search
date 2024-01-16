// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Tree.KeyFrame
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
  public class KeyFrame : Ruleset
  {
    public NodeList Identifiers { get; set; }

    public KeyFrame(NodeList identifiers, NodeList rules)
    {
      this.Identifiers = identifiers;
      this.Rules = rules;
    }

    public override Node Evaluate(Env env)
    {
      env.Frames.Push((Ruleset) this);
      this.Rules = new NodeList(this.Rules.Select<Node, Node>((Func<Node, Node>) (r => r.Evaluate(env)))).ReducedFrom<NodeList>((Node) this.Rules);
      env.Frames.Pop();
      return (Node) this;
    }

    public override void Accept(IVisitor visitor) => this.Rules = this.VisitAndReplace<NodeList>(this.Rules, visitor);

    public override void AppendCSS(Env env, Context context)
    {
      env.Output.AppendMany<Node>((IEnumerable<Node>) this.Identifiers, env.Compress ? "," : ", ");
      if ((bool) (Node) this.Rules.PreComments)
        env.Output.Append((Node) this.Rules.PreComments);
      this.AppendRules(env);
    }
  }
}
