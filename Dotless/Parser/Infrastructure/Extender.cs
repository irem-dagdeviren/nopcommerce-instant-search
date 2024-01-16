// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Extender
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Parser.Tree;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure
{
  public class Extender
  {
    public Selector BaseSelector { get; private set; }

    public List<Selector> ExtendedBy { get; private set; }

    public bool IsReference { get; set; }

    public bool IsMatched { get; set; }

    public Extend Extend { get; private set; }

    [Obsolete("Use the overload that accepts the Extend node")]
    public Extender(Selector baseSelector)
    {
      this.BaseSelector = baseSelector;
      this.ExtendedBy = new List<Selector>();
      this.IsReference = baseSelector.IsReference;
    }

    public Extender(Selector baseSelector, Extend extend)
      : this(baseSelector)
    {
      this.Extend = extend;
    }

    public static string FullPathSelector() => throw new NotImplementedException();

    public void AddExtension(Selector selector, Env env)
    {
      List<IEnumerable<Selector>> selectorPath = new List<IEnumerable<Selector>>()
      {
        (IEnumerable<Selector>) new Selector[1]{ selector }
      };
      selectorPath.AddRange(env.Frames.Skip<Ruleset>(1).Select<Ruleset, IEnumerable<Selector>>((Func<Ruleset, IEnumerable<Selector>>) (f => f.Selectors.Where<Selector>((Func<Selector, bool>) (partialSelector => partialSelector != null)))));
      this.ExtendedBy.Add(this.GenerateExtenderSelector(env, selectorPath));
    }

    private Selector GenerateExtenderSelector(Env env, List<IEnumerable<Selector>> selectorPath)
    {
      Selector extenderSelector = new Selector((IEnumerable<Element>) new Element[1]
      {
        new Element((Combinator) null, this.GenerateExtenderSelector(selectorPath).ToCss(env))
      });
      extenderSelector.IsReference = this.IsReference;
      return extenderSelector;
    }

    private Context GenerateExtenderSelector(List<IEnumerable<Selector>> selectorStack)
    {
      if (!selectorStack.Any<IEnumerable<Selector>>())
        return (Context) null;
      Context extenderSelector1 = this.GenerateExtenderSelector(selectorStack.Skip<IEnumerable<Selector>>(1).ToList<IEnumerable<Selector>>());
      Context extenderSelector2 = new Context();
      extenderSelector2.AppendSelectors(extenderSelector1, selectorStack.First<IEnumerable<Selector>>());
      return extenderSelector2;
    }
  }
}
