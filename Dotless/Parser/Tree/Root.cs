// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Root
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
  public class Root : Ruleset
  {
    public Func<ParsingException, ParserException> Error { get; set; }

    public Root(NodeList rules, Func<ParsingException, ParserException> error)
      : this(rules, error, (Ruleset) null)
    {
    }

    protected Root(NodeList rules, Func<ParsingException, ParserException> error, Ruleset master)
      : base(new NodeList<Selector>(), rules, master)
    {
      this.Error = error;
      this.IsRoot = true;
    }

    public override void AppendCSS(Env env)
    {
      try
      {
        if (this.Rules == null || !this.Rules.Any<Node>())
          return;
        Root root = (Root) this.Evaluate(env);
        root.Rules.InsertRange(0, root.CollectImports().Cast<Node>());
        root.AppendCSS(env, new Context());
      }
      catch (ParsingException ex)
      {
        throw this.Error(ex);
      }
    }

    private IList<Import> CollectImports()
    {
      List<Import> list = this.Rules.OfType<Import>().ToList<Import>();
      foreach (Node node in list)
        this.Rules.Remove(node);
      return (IList<Import>) list;
    }

    private Root DoVisiting(Root node, Env env, VisitorPluginType pluginType) => env.VisitorPlugins.Where<IVisitorPlugin>((Func<IVisitorPlugin, bool>) (p => p.AppliesTo == pluginType)).Aggregate<IVisitorPlugin, Root>(node, (Func<Root, IVisitorPlugin, Root>) ((current, plugin) =>
    {
      try
      {
        plugin.OnPreVisiting(env);
        Root root = plugin.Apply(current);
        plugin.OnPostVisiting(env);
        return root;
      }
      catch (Exception ex)
      {
        throw new ParserException(string.Format("Plugin '{0}' failed during visiting with error '{1}'", (object) plugin.GetName(), (object) ex.Message), ex);
      }
    }));

    public override Node Evaluate(Env env)
    {
      if (this.Evaluated)
        return (Node) this;
      try
      {
        env = env ?? new Env();
        env.Frames.Push((Ruleset) this);
        NodeHelper.ExpandNodes<Import>(env, this.Rules);
        env.Frames.Pop();
        Root node = this.DoVisiting(new Root(new NodeList(this.Rules), this.Error, this.OriginalRuleset), env, VisitorPluginType.BeforeEvaluation);
        node.ReducedFrom<Root>((Node) this);
        node.EvaluateRules(env);
        node.Evaluated = true;
        return (Node) this.DoVisiting(node, env, VisitorPluginType.AfterEvaluation);
      }
      catch (ParsingException ex)
      {
        throw this.Error(ex);
      }
    }
  }
}
