// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Functions.EFunction
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes;
using Nop.Plugin.InstantSearch.Dotless.Parser.Tree;
using Nop.Plugin.InstantSearch.Dotless.Utils;

namespace Nop.Plugin.InstantSearch.Dotless.Parser.Functions
{
  public class EFunction : Function
  {
    protected override Node Evaluate(Env env)
    {
      Guard.ExpectMaxArguments(1, this.Arguments.Count, (object) this, this.Location);
      this.WarnNotSupportedByLessJS("e(string)", "~\"\"");
      if (this.Arguments.Count == 0)
        return (Node) new TextNode("");
      Node node = this.Arguments[0];
      return node is Quoted ? (Node) new TextNode((node as Quoted).UnescapeContents()) : (Node) new TextNode(node.ToCSS(env));
    }
  }
}
