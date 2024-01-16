// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Utils.NodeHelper
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes;
using Nop.Plugin.InstantSearch.Dotless.Parser.Tree;
using System.Collections.Generic;

namespace Nop.Plugin.InstantSearch.Dotless.Utils
{
  internal class NodeHelper
  {
    public static void ExpandNodes<TNode>(Env env, NodeList rules) where TNode : Node
    {
      for (int index = 0; index < rules.Count; ++index)
      {
        Node rule = rules[index];
        if (rule is TNode)
        {
          Node node = rule.Evaluate(env);
          if (node is IEnumerable<Node> collection)
          {
            rules.InsertRange(index + 1, collection);
            rules.RemoveAt(index);
            --index;
          }
          else
            rules[index] = node;
        }
      }
    }

    public static void RecursiveExpandNodes<TNode>(Env env, Ruleset parentRuleset) where TNode : Node
    {
      env.Frames.Push(parentRuleset);
      for (int index = 0; index < parentRuleset.Rules.Count; ++index)
      {
        Node rule = parentRuleset.Rules[index];
        switch (rule)
        {
          case TNode _:
            Node node = rule.Evaluate(env);
            if (node is IEnumerable<Node> collection)
            {
              parentRuleset.Rules.InsertRange(index + 1, collection);
              parentRuleset.Rules.RemoveAt(index);
              --index;
              break;
            }
            parentRuleset.Rules[index] = node;
            break;
          case Ruleset parentRuleset1:
            if (parentRuleset1.Rules != null)
            {
              NodeHelper.RecursiveExpandNodes<TNode>(env, parentRuleset1);
              break;
            }
            break;
        }
      }
      env.Frames.Pop();
    }

    public static IEnumerable<Node> NonDestructiveExpandNodes<TNode>(Env env, NodeList rules) where TNode : Node
    {
      foreach (Node rule in (NodeList<Node>) rules)
      {
        if (rule is TNode)
        {
          foreach (Node node in (IEnumerable<Node>) rule.Evaluate(env))
            yield return node;
        }
        else
          yield return rule;
      }
    }
  }
}
