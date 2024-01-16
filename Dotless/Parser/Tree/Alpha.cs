// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Alpha
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes;
using Nop.Plugin.InstantSearch.Dotless.Plugins;

namespace Nop.Plugin.InstantSearch.Dotless.Parser.Tree
{
  public class Alpha : Call
  {
    public Node Value { get; set; }

    public Alpha(Node value) => this.Value = value;

    public override Node Evaluate(Env env)
    {
      this.Value = this.Value.Evaluate(env);
      return (Node) this;
    }

    public override void AppendCSS(Env env) => env.Output.Append("alpha(opacity=").Append(this.Value).Append(")");

    public override void Accept(IVisitor visitor) => this.Value = this.VisitAndReplace<Node>(this.Value, visitor);
  }
}
