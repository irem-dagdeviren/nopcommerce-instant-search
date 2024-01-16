// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Media
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
  public class Media : Ruleset
  {
    public Node Features { get; set; }

    public Ruleset Ruleset { get; set; }

    public List<Extender> Extensions { get; set; }

    public Media(Node features, NodeList rules)
      : this(features, new Ruleset(Media.GetEmptySelector(), rules), (List<Extender>) null)
    {
    }

    public Media(Node features, Ruleset ruleset, List<Extender> extensions)
    {
      this.Features = features;
      this.Ruleset = ruleset;
      this.Extensions = extensions ?? new List<Extender>();
    }

    protected override Node CloneCore() => (Node) new Media(this.Features.Clone(), (Ruleset) ((Node) this.Ruleset).Clone(), this.Extensions);

    public static NodeList<Selector> GetEmptySelector() => new NodeList<Selector>()
    {
      new Selector((IEnumerable<Element>) new NodeList<Element>()
      {
        new Element(new Combinator(""), "&")
      })
    };

    public override void Accept(IVisitor visitor)
    {
      this.Features = this.VisitAndReplace<Node>(this.Features, visitor);
      this.Ruleset = this.VisitAndReplace<Ruleset>(this.Ruleset, visitor);
    }

    public override Node Evaluate(Env env)
    {
      int count = env.MediaBlocks.Count;
      env.MediaBlocks.Add(this);
      env.MediaPath.Push(this);
      env.Frames.Push(this.Ruleset);
      NodeHelper.ExpandNodes<Import>(env, this.Ruleset.Rules);
      env.Frames.Pop();
      this.Features = this.Features.Evaluate(env);
      Ruleset ruleset = this.Ruleset.Evaluate(env) as Ruleset;
      Media media = new Media(this.Features, ruleset, this.Extensions).ReducedFrom<Media>((Node) this);
      env.MediaPath.Pop();
      env.MediaBlocks[count] = media;
      return env.MediaPath.Count == 0 ? media.EvalTop(env) : media.EvalNested(env, this.Features, ruleset);
    }

    protected Node EvalTop(Env env)
    {
      Node node;
      if (env.MediaBlocks.Count > 1)
        node = (Node) new Ruleset(Media.GetEmptySelector(), new NodeList(env.MediaBlocks.Cast<Node>()))
        {
          MultiMedia = true
        }.ReducedFrom<Ruleset>((Node) this);
      else
        node = (Node) env.MediaBlocks[0];
      env.MediaPath.Clear();
      env.MediaBlocks.Clear();
      return node;
    }

    protected Node EvalNested(Env env, Node features, Ruleset ruleset)
    {
      NodeList<Media> nodeList1 = new NodeList<Media>((IEnumerable<Media>) env.MediaPath.ToList<Media>());
      nodeList1.Add(this);
      NodeList<NodeList> nodeList2 = new NodeList<NodeList>();
      for (int index = 0; index < nodeList1.Count; ++index)
      {
        Node node = !(nodeList1[index].Features is Value features1) ? nodeList1[index].Features : (Node) features1.Values;
        NodeList<NodeList> nodeList3 = nodeList2;
        if (!(node is NodeList nodeList5))
        {
          NodeList nodeList4 = new NodeList();
          nodeList4.Add(node);
          nodeList5 = nodeList4;
        }
        nodeList3.Add(nodeList5);
      }
      NodeList<NodeList> arr = new NodeList<NodeList>();
      foreach (NodeList nodeList6 in nodeList2)
      {
        arr.Add(nodeList6);
        NodeList<NodeList> nodeList7 = arr;
        NodeList nodeList8 = new NodeList();
        nodeList8.Add((Node) new TextNode("and"));
        nodeList7.Add(nodeList8);
      }
      arr.RemoveAt(arr.Count - 1);
      this.Features = (Node) new Value((IEnumerable<Node>) this.Permute(arr), (string) null);
      return (Node) new Ruleset(new NodeList<Selector>(), new NodeList());
    }

    private NodeList Permute(NodeList<NodeList> arr)
    {
      if (arr.Count == 0)
        return new NodeList();
      if (arr.Count == 1)
        return arr[0];
      NodeList nodeList1 = new NodeList();
      NodeList nodeList2 = this.Permute(new NodeList<NodeList>(arr.Skip<NodeList>(1)));
      for (int index1 = 0; index1 < nodeList2.Count; ++index1)
      {
        NodeList nodeList3 = arr[0];
        for (int index2 = 0; index2 < nodeList3.Count; ++index2)
        {
          NodeList nodeList4 = new NodeList();
          nodeList4.Add(nodeList3[index2]);
          NodeList nodeList5 = nodeList2[index1] as NodeList;
          if ((bool) (Node) nodeList5)
            nodeList4.AddRange((IEnumerable<Node>) nodeList5);
          else
            nodeList4.Add(nodeList2[index1]);
          nodeList1.Add((Node) new Expression((IEnumerable<Node>) nodeList4));
        }
      }
      return nodeList1;
    }

    public void BubbleSelectors(NodeList<Selector> selectors)
    {
      NodeList<Selector> selectors1 = new NodeList<Selector>((IEnumerable<Selector>) selectors);
      NodeList rules = new NodeList();
      rules.Add((Node) this.Ruleset);
      this.Ruleset = new Ruleset(selectors1, rules);
    }

    public override void AppendCSS(Env env, Context ctx)
    {
      if (env.Compress && !this.Ruleset.Rules.Any<Node>())
        return;
      env.Output.Push();
      this.Ruleset.IsRoot = ctx.Count == 0;
      env.ExtendMediaScope.Push(this);
      this.Ruleset.AppendCSS(env, ctx);
      env.ExtendMediaScope.Pop();
      if (!env.Compress)
        env.Output.Trim().Indent(2);
      StringBuilder sb = env.Output.Pop();
      if (this.IsReference && this.Ruleset.Rules.All<Node>((Func<Node, bool>) (r => r.IsReference)) || env.Compress && sb.Length == 0)
        return;
      env.Output.Append("@media");
      if ((bool) this.Features)
      {
        env.Output.Append(new char?(' '));
        env.Output.Append(this.Features);
      }
      if (env.Compress)
        env.Output.Append(new char?('{'));
      else
        env.Output.Append(" {\n");
      env.Output.Append(sb);
      if (env.Compress)
        env.Output.Append(new char?('}'));
      else
        env.Output.Append("\n}\n");
    }

    public void AddExtension(Selector selector, Extend extends, Env env)
    {
      foreach (Selector selector1 in extends.Exact)
      {
        Selector extending = selector1;
        Extender extender;
        if ((ExactExtender) (extender = (Extender) this.Extensions.OfType<ExactExtender>().FirstOrDefault<ExactExtender>((Func<ExactExtender, bool>) (e => e.BaseSelector.ToString().Trim() == extending.ToString().Trim()))) == null)
        {
          extender = (Extender) new ExactExtender(extending, extends);
          this.Extensions.Add(extender);
        }
        extender.AddExtension(selector, env);
      }
      foreach (Selector selector2 in extends.Partial)
      {
        Selector extending = selector2;
        Extender extender;
        if ((PartialExtender) (extender = (Extender) this.Extensions.OfType<PartialExtender>().FirstOrDefault<PartialExtender>((Func<PartialExtender, bool>) (e => e.BaseSelector.ToString().Trim() == extending.ToString().Trim()))) == null)
        {
          extender = (Extender) new PartialExtender(extending, extends);
          this.Extensions.Add(extender);
        }
        extender.AddExtension(selector, env);
      }
    }

    public IEnumerable<Extender> FindUnmatchedExtensions() => this.Extensions.Where<Extender>((Func<Extender, bool>) (e => !e.IsMatched));

    public ExactExtender FindExactExtension(string selection) => this.Extensions.OfType<ExactExtender>().FirstOrDefault<ExactExtender>((Func<ExactExtender, bool>) (e => e.BaseSelector.ToString().Trim() == selection));

    public PartialExtender[] FindPartialExtensions(Context selection) => this.Extensions.OfType<PartialExtender>().WhereExtenderMatches(selection).ToArray<PartialExtender>();

    [Obsolete("This method doesn't return the correct results. Use FindPartialExtensions(Context) instead.", false)]
    public PartialExtender[] FindPartialExtensions(string selection) => this.Extensions.OfType<PartialExtender>().Where<PartialExtender>((Func<PartialExtender, bool>) (e => selection.Contains(e.BaseSelector.ToString().Trim()))).ToArray<PartialExtender>();
  }
}
