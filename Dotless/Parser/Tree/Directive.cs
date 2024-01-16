// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Directive
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes;
using Nop.Plugin.InstantSearch.Dotless.Plugins;
using System;
using System.Linq;

namespace Nop.Plugin.InstantSearch.Dotless.Parser.Tree
{
  public class Directive : Ruleset
  {
    public string Name { get; set; }

    public string Identifier { get; set; }

    public Node Value { get; set; }

    public Directive(string name, string identifier, NodeList rules)
    {
      this.Name = name;
      this.Rules = rules;
      this.Identifier = identifier;
    }

    public Directive(string name, Node value)
    {
      this.Name = name;
      this.Value = value;
    }

    protected Directive()
    {
    }

    protected override Node CloneCore() => this.Rules != null ? (Node) new Directive(this.Name, this.Identifier, (NodeList) this.Rules.Clone()) : (Node) new Directive(this.Name, this.Value.Clone());

    public override void Accept(IVisitor visitor)
    {
      this.Rules = this.VisitAndReplace<NodeList>(this.Rules, visitor);
      this.Value = this.VisitAndReplace<Node>(this.Value, visitor);
    }

    public override Node Evaluate(Env env)
    {
      env.Frames.Push((Ruleset) this);
      Directive directive;
      if (this.Rules != null)
        directive = new Directive(this.Name, this.Identifier, new NodeList(this.Rules.Select<Node, Node>((Func<Node, Node>) (r => r.Evaluate(env)))).ReducedFrom<NodeList>((Node) this.Rules));
      else
        directive = new Directive(this.Name, this.Value.Evaluate(env));
      directive.IsReference = this.IsReference;
      env.Frames.Pop();
      return (Node) directive;
    }

    public override void AppendCSS(Env env, Context context)
    {
      if (this.IsReference || env.Compress && this.Rules != null && !this.Rules.Any<Node>())
        return;
      env.Output.Append(this.Name);
      if (!string.IsNullOrEmpty(this.Identifier))
      {
        env.Output.Append(" ");
        env.Output.Append(this.Identifier);
      }
      if (this.Rules != null)
      {
        if ((bool) (Node) this.Rules.PreComments)
          env.Output.Append((Node) this.Rules.PreComments);
        this.AppendRules(env);
        env.Output.Append("\n");
      }
      else
        env.Output.Append(" ").Append(this.Value).Append(";\n");
    }
  }
}
