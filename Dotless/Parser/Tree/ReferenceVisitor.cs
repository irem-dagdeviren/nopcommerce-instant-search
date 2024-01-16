// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Tree.ReferenceVisitor
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes;
using Nop.Plugin.InstantSearch.Dotless.Plugins;

namespace Nop.Plugin.InstantSearch.Dotless.Parser.Tree
{
  public class ReferenceVisitor : IVisitor
  {
    private readonly bool isReference;

    public ReferenceVisitor(bool isReference) => this.isReference = isReference;

    public Node Visit(Node node)
    {
      if (node is Ruleset ruleset)
      {
        if (ruleset.Selectors != null)
        {
          ruleset.Selectors.Accept((IVisitor) this);
          ruleset.Selectors.IsReference = this.isReference;
        }
        if (ruleset.Rules != null)
        {
          ruleset.Rules.Accept((IVisitor) this);
          ruleset.Rules.IsReference = this.isReference;
        }
      }
      if (node is Media media)
      {
        media.Ruleset.Accept((IVisitor) this);
        media.Ruleset.IsReference = this.isReference;
      }
      if (node is NodeList nodeList)
        nodeList.Accept((IVisitor) this);
      node.IsReference = this.isReference;
      return node;
    }
  }
}
