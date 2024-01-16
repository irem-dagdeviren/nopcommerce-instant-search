// Decompiled with JetBrains decompiler
// Type:
// .Parser.Tree.Attribute
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes;

namespace Nop.Plugin.InstantSearch.Dotless.Parser.Tree
{
  public class Attribute : Node
  {
    public Node Name { get; set; }

    public Node Op { get; set; }

    public Node Value { get; set; }

    public Attribute(Node name, Node op, Node value)
    {
      this.Name = name;
      this.Op = op;
      this.Value = value;
    }

    protected override Node CloneCore() => (Node) new Attribute(this.Name.Clone(), this.Op.Clone(), this.Value.Clone());

    public override Node Evaluate(Env env) => (Node) new TextNode(string.Format("[{0}{1}{2}]", (object) this.Name.Evaluate(env).ToCSS(env), this.Op == null ? (object) "" : (object) this.Op.Evaluate(env).ToCSS(env), this.Value == null ? (object) "" : (object) this.Value.Evaluate(env).ToCSS(env)));
  }
}
