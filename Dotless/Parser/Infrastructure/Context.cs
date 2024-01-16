// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Context
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes;
using Nop.Plugin.InstantSearch.Dotless.Parser.Tree;
using Nop.Plugin.InstantSearch.Dotless.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure
{
  public class Context : IEnumerable<IEnumerable<Selector>>, IEnumerable
  {
    private static readonly char[] LeaveUnmerged = new char[3]
    {
      '.',
      '#',
      ':'
    };

    private List<List<Selector>> Paths { get; set; }

    public Context() => this.Paths = new List<List<Selector>>();

    public Context Clone()
    {
      List<List<Selector>> selectorListList = new List<List<Selector>>();
      return new Context() { Paths = selectorListList };
    }

    public void AppendSelectors(Context context, IEnumerable<Selector> selectors)
    {
      foreach (Selector selector in selectors)
        this.AppendSelector(context, selector);
    }

    private void AppendSelector(Context context, Selector selector)
    {
      if (!selector.Elements.Any<Element>((Func<Element, bool>) (e => e.Value == "&")))
      {
        if (context != null && context.Paths.Count > 0)
          this.Paths.AddRange(context.Paths.Select<List<Selector>, List<Selector>>((Func<List<Selector>, List<Selector>>) (path => path.Concat<Selector>((IEnumerable<Selector>) new Selector[1]
          {
            selector
          }).ToList<Selector>())));
        else
          this.Paths.Add(new List<Selector>() { selector });
      }
      else
      {
        NodeList<Element> elements = new NodeList<Element>();
        List<List<Selector>> selectorListList1 = new List<List<Selector>>()
        {
          new List<Selector>()
        };
        foreach (Element element in selector.Elements)
        {
          if (element.Value != "&")
          {
            elements.Add(element);
          }
          else
          {
            List<List<Selector>> selectorListList2 = new List<List<Selector>>();
            if (elements.Count > 0)
              this.MergeElementsOnToSelectors(elements, selectorListList1);
            foreach (List<Selector> source in selectorListList1)
            {
              if (context.Paths.Count == 0)
              {
                if (source.Count > 0)
                {
                  source[0].Elements = new NodeList<Element>((IEnumerable<Element>) source[0].Elements);
                  source[0].Elements.Add(new Element(element.Combinator, ""));
                }
                selectorListList2.Add(source);
              }
              else
              {
                foreach (List<Selector> path in context.Paths)
                {
                  List<Selector> selectorList = new List<Selector>();
                  List<Selector> collection = new List<Selector>();
                  bool flag = true;
                  Selector selector1;
                  if (source.Count > 0)
                  {
                    selector1 = new Selector((IEnumerable<Element>) new NodeList<Element>((IEnumerable<Element>) source[source.Count - 1].Elements));
                    selectorList.AddRange(source.Take<Selector>(source.Count - 1));
                    flag = false;
                  }
                  else
                    selector1 = new Selector((IEnumerable<Element>) new NodeList<Element>());
                  if (path.Count > 1)
                    collection.AddRange(path.Skip<Selector>(1));
                  if (path.Count > 0)
                  {
                    flag = false;
                    if (path[0].Elements[0].Value == null)
                      selector1.Elements.Add(new Element(element.Combinator, path[0].Elements[0].NodeValue));
                    else
                      selector1.Elements.Add(new Element(element.Combinator, path[0].Elements[0].Value));
                    selector1.Elements.AddRange(path[0].Elements.Skip<Element>(1));
                  }
                  if (!flag)
                    selectorList.Add(selector1);
                  selectorList.AddRange((IEnumerable<Selector>) collection);
                  selectorListList2.Add(selectorList);
                }
              }
            }
            selectorListList1 = selectorListList2;
            elements = new NodeList<Element>();
          }
        }
        if (elements.Count > 0)
          this.MergeElementsOnToSelectors(elements, selectorListList1);
        this.Paths.AddRange(selectorListList1.Select<List<Selector>, List<Selector>>((Func<List<Selector>, List<Selector>>) (sel => sel.Select<Selector, Selector>(new Func<Selector, Selector>(this.MergeJoinedElements)).ToList<Selector>())));
      }
    }

    private void MergeElementsOnToSelectors(
      NodeList<Element> elements,
      List<List<Selector>> selectors)
    {
      if (selectors.Count == 0)
      {
        selectors.Add(new List<Selector>()
        {
          new Selector((IEnumerable<Element>) elements)
        });
      }
      else
      {
        foreach (List<Selector> selector in selectors)
        {
          if (selector.Count > 0)
            selector[selector.Count - 1] = new Selector(selector[selector.Count - 1].Elements.Concat<Element>((IEnumerable<Element>) elements));
          else
            selector.Add(new Selector((IEnumerable<Element>) elements));
        }
      }
    }

    private Selector MergeJoinedElements(Selector selector)
    {
      List<Element> list = selector.Elements.Select<Element, Element>((Func<Element, Element>) (e => e.Clone())).ToList<Element>();
      for (int index = 1; index < list.Count; ++index)
      {
        Element element1 = list[index - 1];
        Element element2 = list[index];
        if (!string.IsNullOrEmpty(element2.Value) && !((IEnumerable<char>) Context.LeaveUnmerged).Contains<char>(element2.Value[0]) && !(element2.Combinator.Value != ""))
        {
          list[index - 1] = new Element(element1.Combinator, element1.Value += element2.Value);
          list.RemoveAt(index);
          --index;
        }
      }
      return new Selector((IEnumerable<Element>) list);
    }

    public void AppendCSS(Env env)
    {
      IEnumerable<string> list = this.Paths.Where<List<Selector>>((Func<List<Selector>, bool>) (p => p.Any<Selector>((Func<Selector, bool>) (s => !s.IsReference)))).Select<List<Selector>, string>((Func<List<Selector>, string>) (path => path.Select<Selector, string>((Func<Selector, string>) (p => p.ToCSS(env))).JoinStrings("").Trim())).Distinct<string>();
      env.Output.AppendMany(list, env.Compress ? "," : ",\n");
    }

    public string ToCss(Env env) => string.Join(env.Compress ? "," : ",\n", this.Paths.Select<List<Selector>, string>((Func<List<Selector>, string>) (path => path.Select<Selector, string>((Func<Selector, string>) (p => p.ToCSS(env))).JoinStrings("").Trim())).ToArray<string>());

    public int Count => this.Paths.Count;

    public IEnumerator<IEnumerable<Selector>> GetEnumerator() => this.Paths.Cast<IEnumerable<Selector>>().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
  }
}
