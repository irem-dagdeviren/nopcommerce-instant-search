// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Utils.StringExtensions
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Nop.Plugin.InstantSearch.Dotless.Utils
{
  public static class StringExtensions
  {
    public static string JoinStrings(this IEnumerable<string> source, string separator) => string.Join(separator, source.ToArray<string>());

    public static string AggregatePaths(this IEnumerable<string> source, string currentDirectory) => !source.Any<string>() ? "" : StringExtensions.CanonicalizePath(source.Aggregate<string, string>("", new Func<string, string, string>(Path.Combine)), currentDirectory);

    private static string CanonicalizePath(string path, string currentDirectory)
    {
      Stack<string> source1 = new Stack<string>();
      string str1 = path;
      char[] chArray = new char[2]{ '\\', '/' };
      foreach (string str2 in str1.Split(chArray))
      {
        if (str2.Equals("..") && source1.Count > 0 && source1.Peek() != "..")
          source1.Pop();
        else
          source1.Push(str2);
      }
      IEnumerable<string> source2 = (IEnumerable<string>) source1.Reverse<string>().ToList<string>();
      if (source2.First<string>().Equals(".."))
      {
        int count = source2.TakeWhile<string>((Func<string, bool>) (segment => segment.Equals(".."))).Count<string>();
        IEnumerable<string> strings = ((IEnumerable<string>) currentDirectory.Split('\\', '/')).Reverse<string>().Take<string>(count).Reverse<string>();
        int num1 = 0;
        int num2 = count;
        foreach (string a in strings)
        {
          if (num2 < source2.Count<string>())
          {
            if (string.Equals(a, source2.ElementAt<string>(num2++), StringComparison.InvariantCultureIgnoreCase))
              ++num1;
            else
              break;
          }
          else
            break;
        }
        source2 = source2.Take<string>(count - num1).Concat<string>(source2.Skip<string>(count - num1 + num1 * 2));
      }
      return string.Join("/", source2.ToArray<string>());
    }
  }
}
