// Decompiled with JetBrains decompiler
// Type:
//
// .Parser.Infrastructure.ExactExtender
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Parser.Tree;
using System;

namespace Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure
{
  public class ExactExtender : Extender
  {
    [Obsolete("Use the overload that accepts the Extend node")]
    public ExactExtender(Selector baseSelector)
      : this(baseSelector, (Extend) null)
    {
    }

    public ExactExtender(Selector baseSelector, Extend extend)
      : base(baseSelector, extend)
    {
    }
  }
}
