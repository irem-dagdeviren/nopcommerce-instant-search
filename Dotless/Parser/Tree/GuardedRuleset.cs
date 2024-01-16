// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Tree.GuardedRuleset
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes;
using Nop.Plugin.InstantSearch.Dotless.Plugins;

namespace Nop.Plugin.InstantSearch.Dotless.Parser.Tree
{
  public class GuardedRuleset : Ruleset
  {
    public GuardedRuleset(NodeList<Selector> selectors, NodeList rules, Condition condition)
      : base(selectors, rules)
    {
      this.Condition = condition;
    }

    public Condition Condition { get; set; }

    public override Node Evaluate(Env env) => this.Condition.Passes(env) ? (Node) this.EvaluateRulesForFrame((Ruleset) this, env) : (Node) new NodeList();

    public override void Accept(IVisitor visitor)
    {
      base.Accept(visitor);
      this.Condition = this.VisitAndReplace<Condition>(this.Condition, visitor);
    }

    public override void AppendCSS(Env env)
    {
    }
  }
}
