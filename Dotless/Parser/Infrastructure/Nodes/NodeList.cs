// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes.NodeList
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using System.Collections.Generic;

namespace Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes
{
  public class NodeList : NodeList<Node>
  {
    public NodeList() => this.Inner = new List<Node>();

    public NodeList(params Node[] nodes)
      : this((IEnumerable<Node>) nodes)
    {
    }

    public NodeList(IEnumerable<Node> nodes) => this.Inner = new List<Node>(nodes);

    public NodeList(NodeList nodes)
      : this((IEnumerable<Node>) nodes)
    {
      this.IsReference = nodes.IsReference;
    }
  }
}
