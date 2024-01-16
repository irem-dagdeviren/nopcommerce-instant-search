// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Tree.MixinCall
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Exceptions;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes;
using Nop.Plugin.InstantSearch.Dotless.Plugins;
using Nop.Plugin.InstantSearch.Dotless.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nop.Plugin.InstantSearch.Dotless.Parser.Tree
{
  public class MixinCall : Node
  {
    public List<NamedArgument> Arguments { get; set; }

    public Selector Selector { get; set; }

    public bool Important { get; set; }

    public MixinCall(NodeList<Element> elements, List<NamedArgument> arguments, bool important)
    {
      this.Important = important;
      this.Selector = new Selector((IEnumerable<Element>) elements);
      this.Arguments = arguments;
    }

    protected override Node CloneCore() => (Node) new MixinCall(new NodeList<Element>(this.Selector.Elements.Select<Element, Element>((Func<Element, Element>) (e => e.Clone()))), this.Arguments, this.Important);

    public override Node Evaluate(Env env)
    {
      IEnumerable<Closure> rulesets = env.FindRulesets(this.Selector);
      if (rulesets == null)
        throw new ParsingException(this.Selector.ToCSS(env).Trim() + " is undefined", this.Location);
      env.Rule = (Node) this;
      NodeList rules1 = new NodeList();
      if ((bool) (Node) this.PreComments)
        rules1.AddRange((IEnumerable<Node>) this.PreComments);
      List<Closure> list1 = rulesets.ToList<Closure>();
      List<Closure> list2 = list1.Where<Closure>((Func<Closure, bool>) (c => c.Ruleset is MixinDefinition)).ToList<Closure>();
      List<Closure> closureList = new List<Closure>();
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = false;
      foreach (Closure closure in list2)
      {
        MixinDefinition ruleset = (MixinDefinition) closure.Ruleset;
        MixinMatch mixinMatch = ruleset.MatchArguments(this.Arguments, env);
        switch (mixinMatch)
        {
          case MixinMatch.ArgumentMismatch:
            continue;
          case MixinMatch.Default:
            closureList.Add(closure);
            flag3 = true;
            continue;
          default:
            flag1 = true;
            if (mixinMatch != MixinMatch.GuardFail)
            {
              flag2 = true;
              try
              {
                Env childEnvWithClosure = env.CreateChildEnvWithClosure(closure);
                rules1.AddRange((IEnumerable<Node>) ruleset.Evaluate(this.Arguments, childEnvWithClosure).Rules);
                continue;
              }
              catch (ParsingException ex)
              {
                throw new ParsingException(ex.Message, ex.Location, this.Location);
              }
            }
            else
              continue;
        }
      }
      if (!flag2 & flag3)
      {
        foreach (Closure closure in closureList)
        {
          try
          {
            Env childEnvWithClosure = env.CreateChildEnvWithClosure(closure);
            MixinDefinition ruleset = (MixinDefinition) closure.Ruleset;
            rules1.AddRange((IEnumerable<Node>) ruleset.Evaluate(this.Arguments, childEnvWithClosure).Rules);
          }
          catch (ParsingException ex)
          {
            throw new ParsingException(ex.Message, ex.Location, this.Location);
          }
        }
        flag1 = true;
      }
      if (!flag1)
      {
        foreach (Closure closure in list1.Except<Closure>((IEnumerable<Closure>) list2))
        {
          if (closure.Ruleset.Rules != null)
          {
            NodeList rules2 = (NodeList) closure.Ruleset.Rules.Clone();
            NodeHelper.ExpandNodes<MixinCall>(env, rules2);
            rules1.AddRange((IEnumerable<Node>) rules2);
          }
          flag1 = true;
        }
      }
      if ((bool) (Node) this.PostComments)
        rules1.AddRange((IEnumerable<Node>) this.PostComments);
      env.Rule = (Node) null;
      if (!flag1)
        throw new ParsingException(string.Format("No matching definition was found for `{0}({1})`", (object) this.Selector.ToCSS(env).Trim(), (object) this.Arguments.Select<NamedArgument, string>((Func<NamedArgument, string>) (a => a.Value.ToCSS(env))).JoinStrings(env.Compress ? "," : ", ")), this.Location);
      rules1.Accept((IVisitor) new ReferenceVisitor(this.IsReference));
      return this.Important ? (Node) this.MakeRulesImportant(rules1) : (Node) rules1;
    }

    public override void Accept(IVisitor visitor)
    {
      this.Selector = this.VisitAndReplace<Selector>(this.Selector, visitor);
      foreach (NamedArgument namedArgument in this.Arguments)
        namedArgument.Value = this.VisitAndReplace<Expression>(namedArgument.Value, visitor);
    }

    private Ruleset MakeRulesetImportant(Ruleset ruleset) => new Ruleset(ruleset.Selectors, this.MakeRulesImportant(ruleset.Rules)).ReducedFrom<Ruleset>((Node) ruleset);

    private NodeList MakeRulesImportant(NodeList rules)
    {
      NodeList nodeList = new NodeList();
      foreach (Node rule in (NodeList<Node>) rules)
      {
        switch (rule)
        {
          case MixinCall _:
            MixinCall mixinCall = (MixinCall) rule;
            nodeList.Add((Node) new MixinCall(mixinCall.Selector.Elements, new List<NamedArgument>((IEnumerable<NamedArgument>) mixinCall.Arguments), true).ReducedFrom<MixinCall>(rule));
            continue;
          case Rule _:
            nodeList.Add((Node) this.MakeRuleImportant((Rule) rule));
            continue;
          case Ruleset _:
            nodeList.Add((Node) this.MakeRulesetImportant((Ruleset) rule));
            continue;
          default:
            nodeList.Add(rule);
            continue;
        }
      }
      return nodeList;
    }

    private Rule MakeRuleImportant(Rule rule)
    {
      Node node = rule.Value;
      Value obj1;
      if (!(node is Value obj2))
      {
        NodeList values = new NodeList();
        values.Add(node);
        obj1 = new Value((IEnumerable<Node>) values, "!important");
      }
      else
        obj1 = new Value((IEnumerable<Node>) obj2.Values, "!important").ReducedFrom<Value>((Node) obj2);
      Value obj3 = obj1;
      return new Rule(rule.Name, (Node) obj3).ReducedFrom<Rule>((Node) rule);
    }
  }
}
