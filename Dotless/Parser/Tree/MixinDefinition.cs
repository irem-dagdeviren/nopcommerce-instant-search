// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Tree.MixinDefinition
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Exceptions;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes;
using Nop.Plugin.InstantSearch.Dotless.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nop.Plugin.InstantSearch.Dotless.Parser.Tree
{
  public class MixinDefinition : Ruleset
  {
    private int _required;
    private int _arity;

    public string Name { get; set; }

    public NodeList<Rule> Params { get; set; }

    public Condition Condition { get; set; }

    public bool Variadic { get; set; }

    public MixinDefinition(
      string name,
      NodeList<Rule> parameters,
      NodeList rules,
      Condition condition,
      bool variadic)
    {
      this.Name = name;
      this.Params = parameters;
      this.Rules = rules;
      this.Condition = condition;
      this.Variadic = variadic;
      this.Selectors = new NodeList<Selector>()
      {
        new Selector((IEnumerable<Element>) new NodeList<Element>(new Element[1]
        {
          new Element((Combinator) null, name)
        }))
      };
      this._arity = this.Params.Count;
      this._required = this.Params.Count<Rule>((Func<Rule, bool>) (r => string.IsNullOrEmpty(r.Name) || r.Value == null));
    }

    public override Node Evaluate(Env env) => (Node) this;

    public Ruleset EvaluateParams(Env env, List<NamedArgument> args)
    {
      Dictionary<string, Node> dictionary1 = new Dictionary<string, Node>();
      args = args ?? new List<NamedArgument>();
      bool flag = false;
      foreach (NamedArgument namedArgument in args)
      {
        if (!string.IsNullOrEmpty(namedArgument.Name))
        {
          flag = true;
          Dictionary<string, Node> dictionary2 = dictionary1;
          string name = namedArgument.Name;
          Rule rule = new Rule(namedArgument.Name, namedArgument.Value.Evaluate(env));
          rule.Location = namedArgument.Value.Location;
          dictionary2[name] = (Node) rule;
        }
        else if (flag)
          throw new ParsingException("Positional arguments must appear before all named arguments.", namedArgument.Value.Location);
      }
      for (int index1 = 0; index1 < this.Params.Count; ++index1)
      {
        if (!string.IsNullOrEmpty(this.Params[index1].Name) && !dictionary1.ContainsKey(this.Params[index1].Name))
        {
          Node node1 = index1 >= args.Count || !string.IsNullOrEmpty(args[index1].Name) ? this.Params[index1].Value : (Node) args[index1].Value;
          if (!(bool) node1)
            throw new ParsingException(string.Format("wrong number of arguments for {0} ({1} for {2})", (object) this.Name, (object) (args != null ? args.Count : 0), (object) this._arity), this.Location);
          Node node2;
          if (this.Params[index1].Variadic)
          {
            NodeList nodeList = new NodeList();
            for (int index2 = index1; index2 < args.Count; ++index2)
              nodeList.Add(args[index2].Value.Evaluate(env));
            node2 = new Expression((IEnumerable<Node>) nodeList).Evaluate(env);
          }
          else
            node2 = node1.Evaluate(env);
          Dictionary<string, Node> dictionary3 = dictionary1;
          string name = this.Params[index1].Name;
          Rule rule = new Rule(this.Params[index1].Name, node2);
          rule.Location = node1.Location;
          dictionary3[name] = (Node) rule;
        }
      }
      List<Node> source = new List<Node>();
      for (int index = 0; index < Math.Max(this.Params.Count, args.Count); ++index)
        source.Add(index < args.Count ? (Node) args[index].Value : this.Params[index].Value);
      Ruleset ruleset = new Ruleset(new NodeList<Selector>(), new NodeList());
      ruleset.Rules.Insert(0, (Node) new Rule("@arguments", new Expression(source.Where<Node>((Func<Node, bool>) (a => a != null))).Evaluate(env)));
      foreach (KeyValuePair<string, Node> keyValuePair in dictionary1)
        ruleset.Rules.Add(keyValuePair.Value);
      return ruleset;
    }

    [Obsolete("This method will be removed in a future release. Use Evaluate(List<NamedArgument>, Env) instead.", false)]
    public Ruleset Evaluate(List<NamedArgument> args, Env env, List<Ruleset> closureFrames)
    {
      Env childEnvWithClosure = env.CreateChildEnvWithClosure(new Closure()
      {
        Context = closureFrames,
        Ruleset = (Ruleset) this
      });
      return this.Evaluate(args, childEnvWithClosure);
    }

    public Ruleset Evaluate(List<NamedArgument> args, Env env)
    {
      Ruleset frame = this.EvaluateParams(env, args);
      Env childEnv = env.CreateChildEnv();
      childEnv.Frames.Push((Ruleset) this);
      childEnv.Frames.Push(frame);
      Ruleset rulesForFrame = this.EvaluateRulesForFrame(frame, childEnv);
      childEnv.Frames.Pop();
      childEnv.Frames.Pop();
      return rulesForFrame;
    }

    public override MixinMatch MatchArguments(List<NamedArgument> arguments, Env env)
    {
      int count = arguments != null ? arguments.Count : 0;
      if (!this.Variadic && (count < this._required || count > this._arity))
        return MixinMatch.ArgumentMismatch;
      if ((bool) (Node) this.Condition)
      {
        env.Frames.Push(this.EvaluateParams(env, arguments));
        bool flag = this.Condition.Passes(env);
        env.Frames.Pop();
        if (this.Condition.IsDefault)
          return MixinMatch.Default;
        if (!flag)
          return MixinMatch.GuardFail;
      }
      for (int index = 0; index < Math.Min(count, this._arity); ++index)
      {
        if (string.IsNullOrEmpty(this.Params[index].Name) && arguments[index].Value.Evaluate(env).ToCSS(env) != this.Params[index].Value.Evaluate(env).ToCSS(env))
          return MixinMatch.ArgumentMismatch;
      }
      return MixinMatch.Pass;
    }

    public override void Accept(IVisitor visitor)
    {
      base.Accept(visitor);
      this.Params = this.VisitAndReplace<NodeList<Rule>>(this.Params, visitor);
      this.Condition = this.VisitAndReplace<Condition>(this.Condition, visitor, true);
    }

    public override void AppendCSS(Env env, Context context)
    {
    }
  }
}
