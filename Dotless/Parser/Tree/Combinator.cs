// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Combinator
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes;

namespace Nop.Plugin.InstantSearch.Dotless.Parser.Tree
{
  public class Combinator : Node
  {
    public string Value { get; set; }

    public Combinator(string value)
    {
      if (string.IsNullOrEmpty(value))
        this.Value = "";
      else if (value == " ")
        this.Value = " ";
      else
        this.Value = value.Trim();
    }

    protected override Node CloneCore() => (Node) new Combinator(this.Value);

    public override void AppendCSS(Env env) => env.Output.Append(this.GetValue(env));

    private string GetValue(Env env)
    {
      switch (this.Value)
      {
        case "+":
          return !env.Compress ? " + " : "+";
        case "~":
          return !env.Compress ? " ~ " : "~";
        case ">":
          return !env.Compress ? " > " : ">";
        default:
          return this.Value;
      }
    }
  }
}
