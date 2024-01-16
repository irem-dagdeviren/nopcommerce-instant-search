// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Shorthand
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes;
using Nop.Plugin.InstantSearch.Dotless.Plugins;

namespace Nop.Plugin.InstantSearch.Dotless.Parser.Tree
{
  public class Shorthand : Node
  {
    public Node First { get; set; }

    public Node Second { get; set; }

    public Shorthand(Node first, Node second)
    {
      this.First = first;
      this.Second = second;
    }

    public override Node Evaluate(Env env) => (Node) new Shorthand(this.First.Evaluate(env), this.Second.Evaluate(env));

    protected override Node CloneCore() => (Node) new Shorthand(this.First.Clone(), this.Second.Clone());

    public override void AppendCSS(Env env) => env.Output.Append(this.First).Append("/").Append(this.Second);

    public override void Accept(IVisitor visitor)
    {
      this.First = this.VisitAndReplace<Node>(this.First, visitor);
      this.Second = this.VisitAndReplace<Node>(this.Second, visitor);
    }
  }
}
