// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes.NodeList`1
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Plugins;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes
{
  public class NodeList<TNode> : 
    Node,
    IList<TNode>,
    ICollection<TNode>,
    IEnumerable<TNode>,
    IEnumerable
    where TNode : Node
  {
    protected List<TNode> Inner;

    public NodeList() => this.Inner = new List<TNode>();

    public NodeList(params TNode[] nodes)
      : this((IEnumerable<TNode>) nodes)
    {
    }

    public NodeList(IEnumerable<TNode> nodes) => this.Inner = new List<TNode>(nodes);

    protected override Node CloneCore() => (Node) new NodeList(this.Inner.Select<TNode, Node>((Func<TNode, Node>) (i => i.Clone())));

    public override void AppendCSS(Env env) => env.Output.AppendMany<TNode>((IEnumerable<TNode>) this.Inner);

    public override void Accept(IVisitor visitor)
    {
      List<TNode> nodeList = new List<TNode>(this.Inner.Count);
      foreach (TNode nodeToVisit in this.Inner)
      {
        TNode node = this.VisitAndReplace<TNode>(nodeToVisit, visitor, true);
        if ((object) node != null)
          nodeList.Add(node);
      }
      this.Inner = nodeList;
    }

    public void AddRange(IEnumerable<TNode> nodes) => this.Inner.AddRange(nodes);

    public IEnumerator<TNode> GetEnumerator() => (IEnumerator<TNode>) this.Inner.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public void InsertRange(int index, IEnumerable<TNode> collection) => this.Inner.InsertRange(index, collection);

    public void Add(TNode item) => this.Inner.Add(item);

    public void Clear() => this.Inner.Clear();

    public bool Contains(TNode item) => this.Inner.Contains(item);

    public void CopyTo(TNode[] array, int arrayIndex) => this.Inner.CopyTo(array, arrayIndex);

    public bool Remove(TNode item) => this.Inner.Remove(item);

    public int Count => this.Inner.Count;

    public bool IsReadOnly => ((IList) this.Inner).IsReadOnly;

    public int IndexOf(TNode item) => this.Inner.IndexOf(item);

    public void Insert(int index, TNode item) => this.Inner.Insert(index, item);

    public void RemoveAt(int index) => this.Inner.RemoveAt(index);

    public TNode this[int index]
    {
      get => this.Inner[index];
      set => this.Inner[index] = value;
    }
  }
}
