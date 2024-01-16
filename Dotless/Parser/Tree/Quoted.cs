// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Quoted
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes;
using System.Text;
using System.Text.RegularExpressions;

namespace Nop.Plugin.InstantSearch.Dotless.Parser.Tree
{
  public class Quoted : TextNode
  {
    private readonly Regex _unescape = new Regex("(^|[^\\\\])\\\\(['\"])");

    public char? Quote { get; set; }

    public bool Escaped { get; set; }

    public Quoted(string value, char? quote)
      : base(value)
    {
      this.Quote = quote;
    }

    public Quoted(string value, char? quote, bool escaped)
      : base(value)
    {
      this.Escaped = escaped;
      this.Quote = quote;
    }

    public Quoted(string value, string contents, bool escaped)
      : base(contents)
    {
      this.Escaped = escaped;
      this.Quote = new char?(value[0]);
    }

    public Quoted(string value, bool escaped)
      : base(value)
    {
      this.Escaped = escaped;
      this.Quote = new char?();
    }

    protected override Node CloneCore() => (Node) new Quoted(this.Value, this.Quote, this.Escaped);

    public override void AppendCSS(Env env) => env.Output.Append(this.RenderString());

    public StringBuilder RenderString() => this.Escaped ? new StringBuilder(this.UnescapeContents()) : new StringBuilder().Append((object) this.Quote).Append(this.Value).Append((object) this.Quote);

    public override string ToString() => this.RenderString().ToString();

    public override Node Evaluate(Env env) => (Node) new Quoted(Regex.Replace(this.Value, "@\\{([\\w-]+)\\}", (MatchEvaluator) (m =>
    {
      Node node = new Variable("@" + m.Groups[1].Value)
      {
        Location = new NodeLocation(this.Location.Index + m.Index, this.Location.Source, this.Location.FileName)
      }.Evaluate(env);
      return !(node is TextNode) ? node.ToCSS(env) : (node as TextNode).Value;
    })), this.Quote, this.Escaped).ReducedFrom<Quoted>((Node) this);

    public string UnescapeContents() => this._unescape.Replace(this.Value, "$1$2");
  }
}
