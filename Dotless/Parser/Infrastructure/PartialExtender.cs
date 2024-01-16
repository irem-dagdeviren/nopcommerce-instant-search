// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.PartialExtender
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Parser.Tree;
using System;
using System.Collections.Generic;

namespace Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure
{
  public class PartialExtender : Extender
  {
    [Obsolete("Use the overload that accepts the Extend node")]
    public PartialExtender(Selector baseSelector)
      : this(baseSelector, (Extend) null)
    {
    }

    public PartialExtender(Selector baseSelector, Extend extend)
      : base(baseSelector, extend)
    {
    }

    public IEnumerable<Selector> Replacements(string selection)
    {
      PartialExtender partialExtender = this;
      foreach (Selector selector in partialExtender.ExtendedBy)
        yield return new Selector((IEnumerable<Element>) new Element[1]
        {
          new Element((Combinator) null, selection.Replace(partialExtender.BaseSelector.ToString().Trim(), selector.ToString().Trim()))
        });
    }
  }
}
