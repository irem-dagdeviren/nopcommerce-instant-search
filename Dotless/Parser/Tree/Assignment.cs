// Decompiled with JetBrains decompiler
// Type:
// .Parser.Tree.Assignment
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes;
using Nop.Plugin.InstantSearch.Dotless.Plugins;

namespace Nop.Plugin.InstantSearch.Dotless.Parser.Tree
{
  public class Assignment : Node
  {
    public string Key { get; set; }

    public Node Value { get; set; }

    public Assignment(string key, Node value)
    {
      this.Key = key;
      this.Value = value;
    }

    public override Node Evaluate(Env env)
    {
      Assignment assignment = new Assignment(this.Key, this.Value.Evaluate(env));
      assignment.Location = this.Location;
      return (Node) assignment;
    }

    protected override Node CloneCore() => (Node) new Assignment(this.Key, this.Value.Clone());

    public override void AppendCSS(Env env) => env.Output.Append(this.Key).Append("=").Append(this.Value);

    public override void Accept(IVisitor visitor) => this.Value = this.VisitAndReplace<Node>(this.Value, visitor);
  }
}
