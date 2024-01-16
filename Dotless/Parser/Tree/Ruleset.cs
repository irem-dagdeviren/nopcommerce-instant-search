// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Ruleset
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes;
using Nop.Plugin.InstantSearch.Dotless.Plugins;
using Nop.Plugin.InstantSearch.Dotless.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nop.Plugin.InstantSearch.Dotless.Parser.Tree
{
  public class Ruleset : Node
  {
    private Dictionary<string, List<Closure>> _lookups;

    public NodeList<Selector> Selectors { get; set; }

    public NodeList Rules { get; set; }

    public bool Evaluated { get; protected set; }

    public bool IsRoot { get; set; }

    public bool MultiMedia { get; set; }

    public Ruleset OriginalRuleset { get; set; }

    public Ruleset(NodeList<Selector> selectors, NodeList rules)
      : this(selectors, rules, (Ruleset) null)
    {
    }

    protected Ruleset(NodeList<Selector> selectors, NodeList rules, Ruleset originalRuleset)
      : this()
    {
      this.Selectors = selectors;
      this.Rules = rules;
      this.OriginalRuleset = originalRuleset ?? this;
    }

    protected Ruleset()
    {
      this._lookups = new Dictionary<string, List<Closure>>();
      this.OriginalRuleset = this;
    }

    public bool IsEqualOrClonedFrom(Node node)
    {
      Ruleset ruleset = node as Ruleset;
      return (bool) (Node) ruleset && this.IsEqualOrClonedFrom(ruleset);
    }

    public bool IsEqualOrClonedFrom(Ruleset ruleset) => ruleset.OriginalRuleset == this.OriginalRuleset;

    public Rule Variable(string name, Node startNode)
    {
      Ruleset startNodeRuleset = startNode as Ruleset;
      return this.Rules.TakeWhile<Node>((Func<Node, bool>) (r =>
      {
        if (r == startNode)
          return false;
        return startNodeRuleset == null || !startNodeRuleset.IsEqualOrClonedFrom(r);
      })).OfType<Rule>().Where<Rule>((Func<Rule, bool>) (r => r.Variable)).Reverse<Rule>().FirstOrDefault<Rule>((Func<Rule, bool>) (r => r.Name == name));
    }

    public List<Ruleset> Rulesets() => ((IEnumerable<Node>) this.Rules ?? Enumerable.Empty<Node>()).OfType<Ruleset>().ToList<Ruleset>();

    public List<Closure> Find<TRuleset>(Env env, Selector selector, Ruleset self) where TRuleset : Ruleset
    {
      Context context = new Context();
      context.AppendSelectors(new Context(), (IEnumerable<Selector>) (this.Selectors ?? new NodeList<Selector>()));
      Context source = new Context();
      source.AppendSelectors(context, (IEnumerable<Selector>) new NodeList<Selector>(new Selector[1]
      {
        selector
      }));
      Selector selector1 = source.Select<IEnumerable<Selector>, Selector>((Func<IEnumerable<Selector>, Selector>) (selectors => new Selector(selectors.SelectMany<Selector, Element>((Func<Selector, IEnumerable<Element>>) (s => (IEnumerable<Element>) s.Elements))))).First<Selector>();
      return this.FindInternal(env, selector1, self, context).ToList<Closure>();
    }

    private IEnumerable<Closure> FindInternal(
      Env env,
      Selector selector,
      Ruleset self,
      Context context)
    {
      if (!selector.Elements.Any<Element>())
        return Enumerable.Empty<Closure>();
      string css = selector.ToCSS(env);
      if (this._lookups.ContainsKey(css))
        return (IEnumerable<Closure>) this._lookups[css];
      self = self ?? this;
      List<Closure> closureList = new List<Closure>();
      Selector selector1 = context.Select<IEnumerable<Selector>, Selector>((Func<IEnumerable<Selector>, Selector>) (selectors => new Selector(selectors.SelectMany<Selector, Element>((Func<Selector, IEnumerable<Element>>) (s => (IEnumerable<Element>) s.Elements))))).Where<Selector>((Func<Selector, bool>) (m => m.Elements.IsSubsequenceOf<Element>((IList<Element>) selector.Elements, new Func<Element, Element, bool>(Ruleset.ElementValuesEqual)))).OrderByDescending<Selector, int>((Func<Selector, int>) (m => m.Elements.Count)).FirstOrDefault<Selector>();
      if (selector1 != null && selector1.Elements.Count == selector.Elements.Count)
        closureList.Add(new Closure()
        {
          Context = new List<Ruleset>() { this },
          Ruleset = this
        });
      foreach (Ruleset ruleset in this.Rulesets().Where<Ruleset>((Func<Ruleset, bool>) (rule =>
      {
        if (rule != self)
          return true;
        return rule is MixinDefinition mixinDefinition2 && mixinDefinition2.Condition != null;
      })))
      {
        if (ruleset.Selectors != null)
        {
          Context context1 = new Context();
          context1.AppendSelectors(context, (IEnumerable<Selector>) ruleset.Selectors);
          foreach (Closure closure in ruleset.FindInternal(env, selector, self, context1))
          {
            closure.Context.Insert(0, this);
            closureList.Add(closure);
          }
        }
      }
      return (IEnumerable<Closure>) (this._lookups[css] = closureList);
    }

    private static bool ElementValuesEqual(Element e1, Element e2)
    {
      if (e1.Value == null && e2.Value == null)
        return true;
      return e1.Value != null && e2.Value != null && string.Equals(e1.Value.Trim(), e2.Value.Trim());
    }

    public virtual MixinMatch MatchArguments(List<NamedArgument> arguments, Env env) => arguments != null && arguments.Count != 0 ? MixinMatch.ArgumentMismatch : MixinMatch.Pass;

    public override Node Evaluate(Env env)
    {
      if (this.Evaluated)
        return (Node) this;
      Ruleset ruleset = this.Clone().ReducedFrom<Ruleset>((Node) this);
      ruleset.EvaluateRules(env);
      ruleset.Evaluated = true;
      return (Node) ruleset;
    }

    private Ruleset Clone()
    {
      Ruleset ruleset = new Ruleset(new NodeList<Selector>((IEnumerable<Selector>) this.Selectors), new NodeList(this.Rules), this.OriginalRuleset);
      ruleset.IsReference = this.IsReference;
      return ruleset;
    }

    protected override Node CloneCore() => (Node) new Ruleset(new NodeList<Selector>((IEnumerable<Selector>) this.Selectors), new NodeList(this.Rules), this.OriginalRuleset)
    {
      Evaluated = this.Evaluated,
      IsRoot = this.IsRoot,
      MultiMedia = this.MultiMedia
    };

    public override void Accept(IVisitor visitor)
    {
      this.Selectors = this.VisitAndReplace<NodeList<Selector>>(this.Selectors, visitor);
      this.Rules = this.VisitAndReplace<NodeList>(this.Rules, visitor);
    }

    protected void EvaluateRules(Env env)
    {
      env.Frames.Push(this);
      for (int index = 0; index < this.Selectors.Count; ++index)
      {
        Node node = this.Selectors[index].Evaluate(env);
        if (node is IEnumerable<Selector> collection)
        {
          this.Selectors.RemoveAt(index);
          this.Selectors.InsertRange(index, collection);
        }
        else
          this.Selectors[index] = node as Selector;
      }
      int count = env.MediaBlocks.Count;
      NodeHelper.ExpandNodes<Import>(env, this.Rules);
      NodeHelper.ExpandNodes<MixinCall>(env, this.Rules);
      foreach (Extend extend in this.Rules.OfType<Extend>().ToArray<Extend>())
      {
        foreach (Selector selector in this.Selectors)
        {
          if (env.MediaPath.Any<Media>())
            env.MediaPath.Peek().AddExtension(selector, (Extend) extend.Evaluate(env), env);
          else
            env.AddExtension(selector, (Extend) extend.Evaluate(env), env);
        }
      }
      for (int index = 0; index < this.Rules.Count; ++index)
        this.Rules[index] = this.Rules[index].Evaluate(env);
      for (int index = count; index < env.MediaBlocks.Count; ++index)
        env.MediaBlocks[index].BubbleSelectors(this.Selectors);
      env.Frames.Pop();
    }

    protected Ruleset EvaluateRulesForFrame(Ruleset frame, Env context)
    {
      NodeList rules = new NodeList();
      foreach (Node rule in (NodeList<Node>) this.Rules)
      {
        switch (rule)
        {
          case MixinDefinition _:
            MixinDefinition mixinDefinition = rule as MixinDefinition;
            IEnumerable<Rule> nodes = mixinDefinition.Params.Concat<Rule>(frame.Rules.Cast<Rule>());
            rules.Add((Node) new MixinDefinition(mixinDefinition.Name, new NodeList<Rule>(nodes), mixinDefinition.Rules, mixinDefinition.Condition, mixinDefinition.Variadic));
            continue;
          case Import _:
            Node node = rule.Evaluate(context);
            if (node is NodeList nodeList)
            {
              rules.AddRange((IEnumerable<Node>) nodeList);
              continue;
            }
            rules.Add(node);
            continue;
          case Directive _:
          case Media _:
            rules.Add(rule.Evaluate(context));
            continue;
          case Ruleset _:
            Ruleset ruleset = rule as Ruleset;
            rules.Add(ruleset.Evaluate(context));
            continue;
          case MixinCall _:
            rules.AddRange((IEnumerable<Node>) rule.Evaluate(context));
            continue;
          default:
            rules.Add(rule.Evaluate(context));
            continue;
        }
      }
      return new Ruleset(this.Selectors, rules);
    }

    public override void AppendCSS(Env env)
    {
      if (this.Rules == null || !this.Rules.Any<Node>())
        return;
      ((Ruleset) this.Evaluate(env)).AppendCSS(env, new Context());
    }

    protected void AppendRules(Env env)
    {
      if (env.Compress && this.Rules.Count == 0)
        return;
      env.Output.Append(env.Compress ? "{" : " {\n").Push().AppendMany<Node>((IEnumerable<Node>) this.Rules, "\n").Trim().Indent(env.Compress ? 0 : 2).PopAndAppend();
      if (env.Compress)
        env.Output.TrimRight(new char?(';'));
      env.Output.Append(env.Compress ? "}" : "\n}");
    }

    public virtual void AppendCSS(Env env, Context context)
    {
      List<StringBuilder> buildersToAppend = new List<StringBuilder>();
      int num = 0;
      Context context1 = new Context();
      if (!this.IsRoot)
      {
        if (!env.Compress && env.Debug && this.Location != null)
          env.Output.Append(string.Format("/* {0}:L{1} */\n", (object) this.Location.FileName, (object) Zone.GetLineNumber(this.Location)));
        context1.AppendSelectors(context, (IEnumerable<Selector>) this.Selectors);
      }
      env.Output.Push();
      bool flag = false;
      foreach (Node rule1 in (NodeList<Node>) this.Rules)
      {
        if (!rule1.IgnoreOutput() && (!(rule1 is Comment comment) || comment.IsValidCss))
        {
          if (rule1 is Ruleset ruleset)
          {
            ruleset.AppendCSS(env, context1);
            if (!ruleset.IsReference)
              flag = true;
          }
          else
          {
            Rule rule2 = rule1 as Rule;
            if (!(bool) (Node) rule2 || !rule2.Variable)
            {
              if (!this.IsRoot)
              {
            //    if (!comment)
                  ++num;
                env.Output.Push().Append(rule1);
                buildersToAppend.Add(env.Output.Pop());
              }
              else
              {
                env.Output.Append(rule1);
                if (!env.Compress)
                  env.Output.Append("\n");
              }
            }
          }
        }
      }
      StringBuilder sb = env.Output.Pop();
      if (this.AddExtenders(env, context, context1))
        this.IsReference = false;
      if (!this.IsReference)
      {
        if (this.IsRoot)
          env.Output.AppendMany((IEnumerable<StringBuilder>) buildersToAppend, env.Compress ? "" : "\n");
        else if (num > 0)
        {
          context1.AppendCSS(env);
          env.Output.Append(env.Compress ? "{" : " {\n  ");
          env.Output.AppendMany(buildersToAppend.ConvertAll<string>((Converter<StringBuilder, string>) (stringBuilder => stringBuilder.ToString())).Distinct<string>(), env.Compress ? "" : "\n  ");
          if (env.Compress)
            env.Output.TrimRight(new char?(';'));
          env.Output.Append(env.Compress ? "}" : "\n}\n");
        }
      }
      if (!(!this.IsReference | flag))
        return;
      env.Output.Append(sb);
    }

    private bool AddExtenders(Env env, Context context, Context paths)
    {
      bool flag1 = false;
      foreach (Selector selector in this.Selectors.Where<Selector>((Func<Selector, bool>) (s => s.Elements.First<Element>().Value != null)))
      {
        Context selection = context.Clone();
        selection.AppendSelectors(context, (IEnumerable<Selector>) new Selector[1]
        {
          selector
        });
        string finalString = selection.ToCss(env);
        ExactExtender exactExtension = env.FindExactExtension(finalString);
        if (exactExtension != null)
        {
          exactExtension.IsMatched = true;
          paths.AppendSelectors(context.Clone(), (IEnumerable<Selector>) exactExtension.ExtendedBy);
        }
        PartialExtender[] partialExtensions = env.FindPartialExtensions(selection);
        if (partialExtensions != null)
        {
          foreach (Extender extender in partialExtensions)
            extender.IsMatched = true;
          paths.AppendSelectors(context.Clone(), ((IEnumerable<PartialExtender>) partialExtensions).SelectMany<PartialExtender, Selector>((Func<PartialExtender, IEnumerable<Selector>>) (p => p.Replacements(finalString))));
        }
        bool flag2 = exactExtension != null && exactExtension.ExtendedBy.Any<Selector>((Func<Selector, bool>) (e => !e.IsReference));
        bool flag3 = partialExtensions != null && ((IEnumerable<PartialExtender>) partialExtensions).Any<PartialExtender>((Func<PartialExtender, bool>) (p => p.ExtendedBy.Any<Selector>((Func<Selector, bool>) (e => !e.IsReference))));
        flag1 = flag1 | flag2 | flag3;
      }
      return flag1;
    }

    public override string ToString()
    {
      string format = "{0}{{{1}}}";
      return this.Selectors == null || this.Selectors.Count <= 0 ? string.Format(format, (object) "*", (object) this.Rules.Count) : string.Format(format, (object) this.Selectors.Select<Selector, string>((Func<Selector, string>) (s => s.ToCSS(new Env((Nop.Plugin.InstantSearch.Dotless.Parser.Parser) null)))).JoinStrings(""), (object) this.Rules.Count);
    }
  }
}
