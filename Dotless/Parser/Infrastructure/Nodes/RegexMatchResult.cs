// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes.RegexMatchResult
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using System.Text.RegularExpressions;

namespace Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes
{
  public class RegexMatchResult : TextNode
  {
    public Match Match { get; set; }

    public RegexMatchResult(Match match)
      : base(match.Value)
    {
      this.Match = match;
    }

    public string this[int index]
    {
      get
      {
        string str = this.Match.Groups[index].Value;
        return !string.IsNullOrEmpty(str) ? str : (string) null;
      }
    }
  }
}
