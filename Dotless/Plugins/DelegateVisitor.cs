// Decompiled with JetBrains decompiler
// Type:
// .Plugins.DelegateVisitor
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes;
using Nop.Plugin.InstantSearch.Dotless.Plugins;
using System;
using System.Collections.Generic;

namespace Nop.Plugin.InstantSearch.Dotless.Plugins
{
  public class DelegateVisitor : IVisitor
  {
    private readonly Func<Node, Node> visitor;

    public DelegateVisitor(Func<Node, Node> visitor) => this.visitor = visitor;

    public Node Visit(Node node)
    {
      if (node is IList<Node> nodeList)
      {
        for (int index = 0; index < nodeList.Count; ++index)
          nodeList[index] = this.Visit(nodeList[index]);
      }
      return this.visitor(node);
    }

    public static IVisitor For<TNode>(Func<TNode, Node> projection) where TNode : Node => (IVisitor) new DelegateVisitor((Func<Node, Node>) (node => !(node is TNode node1) ? node : projection(node1)));

    public static IVisitor For<TNode>(Action<TNode> action) where TNode : Node => (IVisitor) new DelegateVisitor((Func<Node, Node>) (node =>
    {
      if (node is TNode node2)
        action(node2);
      return node;
    }));
  }
}
