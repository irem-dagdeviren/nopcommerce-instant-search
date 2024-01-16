// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Plugins.VisitorPlugin
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes;
using Nop.Plugin.InstantSearch.Dotless.Parser.Tree;

namespace Nop.Plugin.InstantSearch.Dotless.Plugins
{
  public abstract class VisitorPlugin : IVisitorPlugin, IPlugin, IVisitor
  {
    public Root Apply(Root tree)
    {
      this.Visit((Node) tree);
      return tree;
    }

    public abstract VisitorPluginType AppliesTo { get; }

    public Node Visit(Node node)
    {
      bool visitDeeper;
      node = this.Execute(node, out visitDeeper);
      if (visitDeeper && node != null)
        node.Accept((IVisitor) this);
      return node;
    }

    public abstract Node Execute(Node node, out bool visitDeeper);

    public virtual void OnPreVisiting(Env env)
    {
    }

    public virtual void OnPostVisiting(Env env)
    {
    }
  }
}
