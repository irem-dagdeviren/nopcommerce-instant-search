// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Utils.Guard
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Exceptions;
using Nop.Plugin.InstantSearch.Dotless.Parser;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nop.Plugin.InstantSearch.Dotless.Utils
{
  public static class Guard
  {
    public static void Expect(string expected, string actual, object @in, NodeLocation location)
    {
      if (!(actual == expected))
        throw new ParsingException(string.Format("Expected '{0}' in {1}, found '{2}'", (object) expected, @in, (object) actual), location);
    }

    [Obsolete("Use Expect(bool, string, NodeLocation) instead")]
    public static void Expect(Func<bool> condition, string message, NodeLocation location)
    {
      if (!condition())
        throw new ParsingException(message, location);
    }

    public static void Expect(bool condition, string message, NodeLocation location)
    {
      if (!condition)
        throw new ParsingException(message, location);
    }

    public static TExpected ExpectNode<TExpected>(Node actual, object @in, NodeLocation location) where TExpected : Node => actual is TExpected expected ? expected : throw new ParsingException(string.Format("Expected {0} in {1}, found {2}", (object) typeof (TExpected).Name.ToLowerInvariant(), @in, (object) actual.ToCSS(new Env((Nop.Plugin.InstantSearch.Dotless.Parser.Parser) null))), location);

    public static void ExpectNodeToBeOneOf<TExpected1, TExpected2>(
      Node actual,
      object @in,
      NodeLocation location)
      where TExpected1 : Node
      where TExpected2 : Node
    {
      switch (actual)
      {
        case TExpected1 _:
          break;
        case TExpected2 _:
          break;
        default:
          throw new ParsingException(string.Format("Expected {0} or {1} in {2}, found {3}", (object) typeof (TExpected1).Name.ToLowerInvariant(), (object) typeof (TExpected2).Name.ToLowerInvariant(), @in, (object) actual.ToCSS(new Env((Nop.Plugin.InstantSearch.Dotless.Parser.Parser) null))), location);
      }
    }

    public static List<TExpected> ExpectAllNodes<TExpected>(
      IEnumerable<Node> actual,
      object @in,
      NodeLocation location)
      where TExpected : Node
    {
      return actual.Select<Node, TExpected>((Func<Node, TExpected>) (node => Guard.ExpectNode<TExpected>(node, @in, location))).ToList<TExpected>();
    }

    public static void ExpectNumArguments(
      int expected,
      int actual,
      object @in,
      NodeLocation location)
    {
      if (actual != expected)
        throw new ParsingException(string.Format("Expected {0} arguments in {1}, found {2}", (object) expected, @in, (object) actual), location);
    }

    public static void ExpectMinArguments(
      int expected,
      int actual,
      object @in,
      NodeLocation location)
    {
      if (actual < expected)
        throw new ParsingException(string.Format("Expected at least {0} arguments in {1}, found {2}", (object) expected, @in, (object) actual), location);
    }

    public static void ExpectMaxArguments(
      int expected,
      int actual,
      object @in,
      NodeLocation location)
    {
      if (actual > expected)
        throw new ParsingException(string.Format("Expected at most {0} arguments in {1}, found {2}", (object) expected, @in, (object) actual), location);
    }
  }
}
