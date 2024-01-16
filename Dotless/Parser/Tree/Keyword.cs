// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Keyword
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes;
using System;

namespace Nop.Plugin.InstantSearch.Dotless.Parser.Tree
{
  public class Keyword : Node, IComparable
  {
    public string Value { get; set; }

    public Keyword(string value) => this.Value = value;

    public override Node Evaluate(Env env) => ((Node) Color.GetColorFromKeyword(this.Value) ?? (Node) this).ReducedFrom<Node>((Node) this);

    protected override Node CloneCore() => (Node) new Keyword(this.Value);

    public override void AppendCSS(Env env) => env.Output.Append(this.Value);

    public override string ToString() => this.Value;

    public int CompareTo(object obj) => obj == null ? -1 : obj.ToString().CompareTo(this.ToString());
  }
}
