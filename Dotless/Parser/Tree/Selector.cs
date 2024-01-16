// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Selector
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
  public class Selector : Node
  {
    [ThreadStatic]
    private static Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser;
    [ThreadStatic]
    private static Parsers parsers;

    public NodeList<Element> Elements { get; set; }

    public Selector(IEnumerable<Element> elements)
    {
      if (elements is NodeList<Element>)
        this.Elements = elements as NodeList<Element>;
      else
        this.Elements = new NodeList<Element>(elements);
    }

    public bool Match(Selector other) => other.Elements.Count <= this.Elements.Count && this.Elements[0].Value == other.Elements[0].Value;

    private Nop.Plugin.InstantSearch.Dotless.Parser.Parser Parser
    {
      get
      {
        if (Selector.parser == null)
          Selector.parser = new Nop.Plugin.InstantSearch.Dotless.Parser.Parser();
        return Selector.parser;
      }
    }

    private Parsers Parsers
    {
      get
      {
        if (Selector.parsers == null)
          Selector.parsers = new Parsers(this.Parser.NodeProvider);
        return Selector.parsers;
      }
    }

    public override Node Evaluate(Env env)
    {
      NodeList<Element> nodeList1 = new NodeList<Element>();
      foreach (Element element in this.Elements)
      {
        if (element.NodeValue is Extend)
        {
          if (env.MediaPath.Any<Media>())
            env.MediaPath.Peek().AddExtension(this, (Extend) element.NodeValue.Evaluate(env), env);
          else
            env.AddExtension(this, (Extend) element.NodeValue.Evaluate(env), env);
        }
        else
          nodeList1.Add(element.Evaluate(env) as Element);
      }
      Selector selector1 = new Selector((IEnumerable<Element>) nodeList1).ReducedFrom<Selector>((Node) this);
      if (selector1.Elements.All<Element>((Func<Element, bool>) (e => e.NodeValue == null)))
        return (Node) selector1;
      this.Parser.Tokenizer.SetupInput(selector1.ToCSS(env), "");
      NodeList<Selector> nodeList2 = new NodeList<Selector>();
      Selector selector2;
      while ((bool) (Node) (selector2 = this.Parsers.Selector(this.Parser)))
      {
        selector2.IsReference = this.IsReference;
        nodeList2.Add(selector2.Evaluate(env) as Selector);
        if (!(Node) this.Parser.Tokenizer.Match(','))
          break;
      }
      return (Node) nodeList2;
    }

    protected override Node CloneCore() => (Node) new Selector(this.Elements.Select<Element, Element>((Func<Element, Element>) (e => e.Clone())));

    public override void AppendCSS(Env env)
    {
      env.Output.Push();
      if (this.Elements[0].Combinator.Value == "")
        env.Output.Append(new char?(' '));
      env.Output.Append((Node) this.Elements);
      env.Output.Append(env.Output.Pop().ToString());
    }

    public override void Accept(IVisitor visitor) => this.Elements = this.VisitAndReplace<NodeList<Element>>(this.Elements, visitor);

    public override string ToString() => this.ToCSS(new Env((Nop.Plugin.InstantSearch.Dotless.Parser.Parser) null));
  }
}
