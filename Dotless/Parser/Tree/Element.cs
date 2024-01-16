// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Element
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes;
using Nop.Plugin.InstantSearch.Dotless.Plugins;

namespace Nop.Plugin.InstantSearch.Dotless.Parser.Tree
{
  public class Element : Node
  {
    public Combinator Combinator { get; set; }

    public string Value { get; set; }

    public Node NodeValue { get; set; }

    public Element(Combinator combinator, string textValue)
      : this(combinator)
    {
      this.Value = textValue.Trim();
    }

    public Element(Combinator combinator, Node value)
      : this(combinator)
    {
      if (value is TextNode textNode && !(value is Quoted))
        this.Value = textNode.Value.Trim();
      else
        this.NodeValue = value;
    }

    private Element(Combinator combinator) => this.Combinator = combinator ?? new Combinator("");

    public override Node Evaluate(Env env)
    {
      if (this.NodeValue == null)
        return (Node) this;
      return (Node) new Element(this.Combinator, this.NodeValue.Evaluate(env)).ReducedFrom<Element>((Node) this);
    }

    protected override Node CloneCore() => this.NodeValue != null ? (Node) new Element((Combinator) this.Combinator.Clone(), this.NodeValue.Clone()) : (Node) new Element((Combinator) this.Combinator.Clone(), this.Value);

    public override void AppendCSS(Env env)
    {
      env.Output.Append((Node) this.Combinator).Push();
      if (this.NodeValue != null)
        env.Output.Append(this.NodeValue).Trim();
      else
        env.Output.Append(this.Value);
      env.Output.PopAndAppend();
    }

    public override void Accept(IVisitor visitor)
    {
      this.Combinator = this.VisitAndReplace<Combinator>(this.Combinator, visitor);
      this.NodeValue = this.VisitAndReplace<Node>(this.NodeValue, visitor, true);
    }

    internal Element Clone() => new Element(this.Combinator)
    {
      Value = this.Value,
      NodeValue = this.NodeValue
    };
  }
}
