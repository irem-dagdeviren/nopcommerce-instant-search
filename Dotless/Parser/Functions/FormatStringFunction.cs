// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Functions.FormatStringFunction
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Exceptions;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes;
using Nop.Plugin.InstantSearch.Dotless.Parser.Tree;
using System;
using System.Linq;

namespace Nop.Plugin.InstantSearch.Dotless.Parser.Functions
{
  public class FormatStringFunction : Function
  {
    protected override Node Evaluate(Env env)
    {
      this.WarnNotSupportedByLessJS("formatstring(string, args...)", (string) null, " You may want to consider using string interpolation (\"@{variable}\") which does the same thing and is supported.");
      if (this.Arguments.Count == 0)
        return (Node) new Quoted("", false);
      Func<Node, string> selector = (Func<Node, string>) (n => !(n is Quoted) ? n.ToCSS(env) : ((Quoted) n).UnescapeContents());
      string format = selector(this.Arguments[0]);
      string[] array = this.Arguments.Skip<Node>(1).Select<Node, string>(selector).ToArray<string>();
      string str;
      try
      {
        str = string.Format(format, (object[]) array);
      }
      catch (FormatException ex)
      {
        throw new ParserException(string.Format("Error in formatString :{0}", (object) ex.Message), (Exception) ex);
      }
      return (Node) new Quoted(str, false);
    }
  }
}
