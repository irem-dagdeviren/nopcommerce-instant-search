// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Paren
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes;
using Nop.Plugin.InstantSearch.Dotless.Plugins;

namespace Nop.Plugin.InstantSearch.Dotless.Parser.Tree
{
  public class Paren : Node
  {
    public Node Value { get; set; }

    public Paren(Node value) => this.Value = value;

    protected override Node CloneCore() => (Node) new Paren(this.Value.Clone());

    public override void AppendCSS(Env env) => env.Output.Append(new char?('(')).Append(this.Value).Append(new char?(')'));

    public override Node Evaluate(Env env) => (Node) new Paren(this.Value.Evaluate(env));

    public override void Accept(IVisitor visitor) => this.Value = this.VisitAndReplace<Node>(this.Value, visitor);
  }
}
