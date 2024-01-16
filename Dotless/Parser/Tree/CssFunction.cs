// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Tree.CssFunction
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes;

namespace Nop.Plugin.InstantSearch.Dotless.Parser.Tree
{
  public class CssFunction : Node
  {
    public string Name { get; set; }

    public Node Value { get; set; }

    protected override Node CloneCore() => (Node) new CssFunction()
    {
      Name = this.Name,
      Value = this.Value.Clone()
    };

    public override void AppendCSS(Env env) => env.Output.Append(string.Format("{0}({1})", (object) this.Name, (object) this.Value.ToCSS(env)));

    public override Node Evaluate(Env env)
    {
      CssFunction cssFunction = (CssFunction) this.Clone();
      cssFunction.Value = this.Value.Evaluate(env);
      return (Node) cssFunction;
    }
  }
}
