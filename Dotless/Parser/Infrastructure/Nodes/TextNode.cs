// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes.TextNode
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using System;

namespace Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes
{
  public class TextNode : Node, IComparable
  {
    public string Value { get; set; }

    public TextNode(string contents) => this.Value = contents;

    public static TextNode operator &(TextNode n1, TextNode n2) => n1 == null ? (TextNode) null : n2;

    public static TextNode operator |(TextNode n1, TextNode n2) => n1 ?? n2;

    protected override Node CloneCore() => (Node) new TextNode(this.Value);

    public override void AppendCSS(Env env) => env.Output.Append(env.Compress ? this.Value.Trim() : this.Value);

    public override string ToString() => this.Value;

    public virtual int CompareTo(object obj) => obj == null ? -1 : obj.ToString().CompareTo(this.ToString());
  }
}
