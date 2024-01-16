// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Functions.CFormatString
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes;
using Nop.Plugin.InstantSearch.Dotless.Parser.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Nop.Plugin.InstantSearch.Dotless.Parser.Functions
{
  public class CFormatString : Function
  {
    protected override Node Evaluate(Env env)
    {
      this.WarnNotSupportedByLessJS("%(string, args...)", "~\"\" and string interpolation");
      if (this.Arguments.Count == 0)
        return (Node) new Quoted("", false);
      Func<Node, string> stringValue = (Func<Node, string>) (n => !(n is Quoted) ? n.ToCSS(env) : ((TextNode) n).Value);
      string input = stringValue(this.Arguments[0]);
      List<Node> args = this.Arguments.Skip<Node>(1).ToList<Node>();
      int i = 0;
      MatchEvaluator evaluator = (MatchEvaluator) (m =>
      {
        string stringToEscape = m.Value == "%s" ? stringValue(args[i++]) : args[i++].ToCSS(env);
        return !char.IsUpper(m.Value[1]) ? stringToEscape : Uri.EscapeDataString(stringToEscape);
      });
      return (Node) new Quoted(Regex.Replace(input, "%[sda]", evaluator, RegexOptions.IgnoreCase), this.Arguments[0] is Quoted ? (this.Arguments[0] as Quoted).Quote : new char?());
    }
  }
}
