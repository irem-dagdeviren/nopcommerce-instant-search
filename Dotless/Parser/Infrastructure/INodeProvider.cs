// Decompiled with JetBrains decompiler
// Type:
// .Parser.Infrastructure.INodeProvider
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Importers;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes;
using Nop.Plugin.InstantSearch.Dotless.Parser.Tree;
using System.Collections.Generic;

namespace Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure
{
  public interface INodeProvider
  {
    Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Element Element(
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Combinator combinator,
      Node Value,
      NodeLocation location);

    Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Combinator Combinator(string value, NodeLocation location);

    Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Selector Selector(NodeList<Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Element> elements, NodeLocation location);

    Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Rule Rule(string name, Node value, NodeLocation location);

    Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Rule Rule(
      string name,
      Node value,
      bool variadic,
      NodeLocation location);

    Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Ruleset Ruleset(
      NodeList<Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Selector> selectors,
      NodeList rules,
      NodeLocation location);

    Nop.Plugin.InstantSearch.Dotless.Parser.Tree.CssFunction CssFunction(
      string name,
      Node value,
      NodeLocation location);

    Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Alpha Alpha(Node value, NodeLocation location);

    Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Call Call(
      string name,
      NodeList<Node> arguments,
      NodeLocation location);

    Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Color Color(string rgb, NodeLocation location);

    Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Keyword Keyword(string value, NodeLocation location);

    Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Number Number(string value, string unit, NodeLocation location);

    Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Shorthand Shorthand(Node first, Node second, NodeLocation location);

    Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Variable Variable(string name, NodeLocation location);

    Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Url Url(Node value, IImporter importer, NodeLocation location);

    Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Script Script(string script, NodeLocation location);

    Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Paren Paren(Node node, NodeLocation location);

    Nop.Plugin.InstantSearch.Dotless.Parser.Tree.GuardedRuleset GuardedRuleset(
      NodeList<Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Selector> selectors,
      NodeList rules,
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Condition condition,
      NodeLocation location);

    Nop.Plugin.InstantSearch.Dotless.Parser.Tree.MixinCall MixinCall(
      NodeList<Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Element> elements,
      List<NamedArgument> arguments,
      bool important,
      NodeLocation location);

    Nop.Plugin.InstantSearch.Dotless.Parser.Tree.MixinDefinition MixinDefinition(
      string name,
      NodeList<Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Rule> parameters,
      NodeList rules,
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Condition condition,
      bool variadic,
      NodeLocation location);

    Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Condition Condition(
      Node left,
      string operation,
      Node right,
      bool negate,
      NodeLocation location);

    Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Import Import(
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Url path,
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Value features,
      ImportOptions option,
      NodeLocation location);

    Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Import Import(
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Quoted path,
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Value features,
      ImportOptions option,
      NodeLocation location);

    Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Directive Directive(
      string name,
      string identifier,
      NodeList rules,
      NodeLocation location);

    Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Directive Directive(string name, Node value, NodeLocation location);

    Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Media Media(NodeList rules, Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Value features, NodeLocation location);

    Nop.Plugin.InstantSearch.Dotless.Parser.Tree.KeyFrame KeyFrame(
      NodeList identifier,
      NodeList rules,
      NodeLocation location);

    Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Expression Expression(NodeList expression, NodeLocation location);

    Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Value Value(
      IEnumerable<Node> values,
      string important,
      NodeLocation location);

    Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Operation Operation(
      string operation,
      Node left,
      Node right,
      NodeLocation location);

    Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Assignment Assignment(string key, Node value, NodeLocation location);

    Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Comment Comment(string value, NodeLocation location);

    Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes.TextNode TextNode(
      string contents,
      NodeLocation location);

    Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Quoted Quoted(
      string value,
      string contents,
      bool escaped,
      NodeLocation location);

    Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Extend Extend(
      List<Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Selector> exact,
      List<Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Selector> partial,
      NodeLocation location);

    Node Attribute(Node key, Node op, Node val, NodeLocation location);
  }
}
