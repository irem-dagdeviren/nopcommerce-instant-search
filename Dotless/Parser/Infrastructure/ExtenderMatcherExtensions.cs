// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.ExtenderMatcherExtensions
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Parser.Tree;
using Nop.Plugin.InstantSearch.Dotless.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure
{
  internal static class ExtenderMatcherExtensions
  {
    internal static IEnumerable<PartialExtender> WhereExtenderMatches(
      this IEnumerable<PartialExtender> extenders,
      Context selection)
    {
      List<Element> selectionElements = selection.SelectMany<IEnumerable<Selector>, Element>((Func<IEnumerable<Selector>, IEnumerable<Element>>) (selectors => selectors.SelectMany<Selector, Element>((Func<Selector, IEnumerable<Element>>) (s => (IEnumerable<Element>) s.Elements)))).ToList<Element>();
      return extenders.Where<PartialExtender>((Func<PartialExtender, bool>) (e => e.ElementListMatches((IList<Element>) selectionElements)));
    }

    private static bool ElementListMatches(this PartialExtender extender, IList<Element> list)
    {
      int count = extender.BaseSelector.Elements.Count;
      return extender.BaseSelector.Elements.IsSubsequenceOf<Element>(list, (Func<int, Element, int, Element, bool>) ((subIndex, subElement, index, seqelement) => subIndex < count - 1 ? object.Equals((object) subElement.Combinator, (object) seqelement.Combinator) && string.Equals(subElement.Value, seqelement.Value) && object.Equals((object) subElement.NodeValue, (object) seqelement.NodeValue) : string.Equals(subElement.Value, seqelement.Value) && object.Equals((object) subElement.NodeValue, (object) seqelement.NodeValue)));
    }
  }
}
