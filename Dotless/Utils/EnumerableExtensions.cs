// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Utils.EnumerableExtensions
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using System;
using System.Collections.Generic;

namespace Nop.Plugin.InstantSearch.Dotless.Utils
{
  internal static class EnumerableExtensions
  {
    internal static bool IsSubsequenceOf<TElement>(
      this IList<TElement> subsequence,
      IList<TElement> sequence)
    {
      return subsequence.IsSubsequenceOf<TElement>(sequence, (Func<TElement, TElement, bool>) ((element1, element2) => object.Equals((object) element1, (object) element2)));
    }

    internal static bool IsSubsequenceOf<TElement>(
      this IList<TElement> subsequence,
      IList<TElement> sequence,
      Func<TElement, TElement, bool> areEqual)
    {
      return subsequence.IsSubsequenceOf<TElement>(sequence, (Func<int, TElement, int, TElement, bool>) ((_, element1, __, element2) => areEqual(element1, element2)));
    }

    internal static bool IsSubsequenceOf<TElement>(
      this IList<TElement> subsequence,
      IList<TElement> sequence,
      Func<int, TElement, int, TElement, bool> areEqual)
    {
      if (subsequence.Count == 0)
        return true;
      if (sequence.Count == 0)
        return false;
      int index1 = 0;
      while (!areEqual(0, subsequence[0], index1, sequence[index1]))
      {
        ++index1;
        if (index1 >= sequence.Count)
          return false;
      }
      int index2 = 0;
      do
      {
        ++index1;
        ++index2;
        if (index2 >= subsequence.Count)
          return true;
      }
      while (index1 < sequence.Count && areEqual(index2, subsequence[index2], index1, sequence[index1]));
      return false;
    }
  }
}
