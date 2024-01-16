// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Plugins.IVisitorPlugin
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure;
using Nop.Plugin.InstantSearch.Dotless.Parser.Tree;

namespace Nop.Plugin.InstantSearch.Dotless.Plugins
{
  public interface IVisitorPlugin : IPlugin
  {
    Root Apply(Root tree);

    VisitorPluginType AppliesTo { get; }

    void OnPreVisiting(Env env);

    void OnPostVisiting(Env env);
  }
}
