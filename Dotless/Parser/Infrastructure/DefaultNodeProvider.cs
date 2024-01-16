// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.DefaultNodeProvider
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Importers;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes;
using Nop.Plugin.InstantSearch.Dotless.Parser.Tree;
using System.Collections.Generic;

namespace Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure
{
  public class DefaultNodeProvider : INodeProvider
  {
    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Element Element(
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Combinator combinator,
      Node value,
      NodeLocation location)
    {
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Element element = new Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Element(combinator, value);
      element.Location = location;
      return element;
    }

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Combinator Combinator(string value, NodeLocation location)
    {
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Combinator combinator = new Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Combinator(value);
      combinator.Location = location;
      return combinator;
    }

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Selector Selector(
      NodeList<Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Element> elements,
      NodeLocation location)
    {
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Selector selector = new Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Selector((IEnumerable<Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Element>) elements);
      selector.Location = location;
      return selector;
    }

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Rule Rule(string name, Node value, NodeLocation location)
    {
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Rule rule = new Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Rule(name, value);
      rule.Location = location;
      return rule;
    }

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Rule Rule(
      string name,
      Node value,
      bool variadic,
      NodeLocation location)
    {
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Rule rule = new Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Rule(name, value, variadic);
      rule.Location = location;
      return rule;
    }

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Ruleset Ruleset(
      NodeList<Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Selector> selectors,
      NodeList rules,
      NodeLocation location)
    {
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Ruleset ruleset = new Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Ruleset(selectors, rules);
      ruleset.Location = location;
      return ruleset;
    }

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.CssFunction CssFunction(
      string name,
      Node value,
      NodeLocation location)
    {
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.CssFunction cssFunction = new Nop.Plugin.InstantSearch.Dotless.Parser.Tree.CssFunction();
      cssFunction.Name = name;
      cssFunction.Value = value;
      cssFunction.Location = location;
      return cssFunction;
    }

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Alpha Alpha(Node value, NodeLocation location)
    {
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Alpha alpha = new Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Alpha(value);
      alpha.Location = location;
      return alpha;
    }

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Call Call(
      string name,
      NodeList<Node> arguments,
      NodeLocation location)
    {
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Call call = new Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Call(name, arguments);
      call.Location = location;
      return call;
    }

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Color Color(string rgb, NodeLocation location)
    {
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Color color = Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Color.FromHex(rgb);
      color.Location = location;
      return color;
    }

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Keyword Keyword(string value, NodeLocation location)
    {
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Keyword keyword = new Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Keyword(value);
      keyword.Location = location;
      return keyword;
    }

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Number Number(string value, string unit, NodeLocation location)
    {
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Number number = new Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Number(value, unit);
      number.Location = location;
      return number;
    }

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Shorthand Shorthand(
      Node first,
      Node second,
      NodeLocation location)
    {
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Shorthand shorthand = new Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Shorthand(first, second);
      shorthand.Location = location;
      return shorthand;
    }

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Variable Variable(string name, NodeLocation location)
    {
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Variable variable = new Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Variable(name);
      variable.Location = location;
      return variable;
    }

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Url Url(Node value, IImporter importer, NodeLocation location)
    {
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Url url = new Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Url(value, importer);
      url.Location = location;
      return url;
    }

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Script Script(string script, NodeLocation location)
    {
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Script script1 = new Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Script(script);
      script1.Location = location;
      return script1;
    }

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.GuardedRuleset GuardedRuleset(
      NodeList<Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Selector> selectors,
      NodeList rules,
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Condition condition,
      NodeLocation location)
    {
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.GuardedRuleset guardedRuleset = new Nop.Plugin.InstantSearch.Dotless.Parser.Tree.GuardedRuleset(selectors, rules, condition);
      guardedRuleset.Location = location;
      return guardedRuleset;
    }

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.MixinCall MixinCall(
      NodeList<Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Element> elements,
      List<NamedArgument> arguments,
      bool important,
      NodeLocation location)
    {
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.MixinCall mixinCall = new Nop.Plugin.InstantSearch.Dotless.Parser.Tree.MixinCall(elements, arguments, important);
      mixinCall.Location = location;
      return mixinCall;
    }

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.MixinDefinition MixinDefinition(
      string name,
      NodeList<Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Rule> parameters,
      NodeList rules,
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Condition condition,
      bool variadic,
      NodeLocation location)
    {
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.MixinDefinition mixinDefinition = new Nop.Plugin.InstantSearch.Dotless.Parser.Tree.MixinDefinition(name, parameters, rules, condition, variadic);
      mixinDefinition.Location = location;
      return mixinDefinition;
    }

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Import Import(
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Url path,
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Value features,
      ImportOptions option,
      NodeLocation location)
    {
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Import import = new Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Import(path, features, option);
      import.Location = location;
      return import;
    }

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Import Import(
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Quoted path,
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Value features,
      ImportOptions option,
      NodeLocation location)
    {
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Import import = new Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Import(path, features, option);
      import.Location = location;
      return import;
    }

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Directive Directive(
      string name,
      string identifier,
      NodeList rules,
      NodeLocation location)
    {
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Directive directive = new Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Directive(name, identifier, rules);
      directive.Location = location;
      return directive;
    }

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Media Media(
      NodeList rules,
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Value features,
      NodeLocation location)
    {
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Media media = new Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Media((Node) features, rules);
      media.Location = location;
      return media;
    }

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.KeyFrame KeyFrame(
      NodeList identifier,
      NodeList rules,
      NodeLocation location)
    {
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.KeyFrame keyFrame = new Nop.Plugin.InstantSearch.Dotless.Parser.Tree.KeyFrame(identifier, rules);
      keyFrame.Location = location;
      return keyFrame;
    }

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Directive Directive(
      string name,
      Node value,
      NodeLocation location)
    {
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Directive directive = new Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Directive(name, value);
      directive.Location = location;
      return directive;
    }

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Expression Expression(
      NodeList expression,
      NodeLocation location)
    {
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Expression expression1 = new Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Expression((IEnumerable<Node>) expression);
      expression1.Location = location;
      return expression1;
    }

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Value Value(
      IEnumerable<Node> values,
      string important,
      NodeLocation location)
    {
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Value obj = new Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Value(values, important);
      obj.Location = location;
      return obj;
    }

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Operation Operation(
      string operation,
      Node left,
      Node right,
      NodeLocation location)
    {
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Operation operation1 = new Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Operation(operation, left, right);
      operation1.Location = location;
      return operation1;
    }

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Assignment Assignment(
      string key,
      Node value,
      NodeLocation location)
    {
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Assignment assignment = new Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Assignment(key, value);
      assignment.Location = location;
      return assignment;
    }

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Comment Comment(string value, NodeLocation location)
    {
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Comment comment = new Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Comment(value);
      comment.Location = location;
      return comment;
    }

    public Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes.TextNode TextNode(
      string contents,
      NodeLocation location)
    {
      Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes.TextNode textNode = new Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes.TextNode(contents);
      textNode.Location = location;
      return textNode;
    }

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Quoted Quoted(
      string value,
      string contents,
      bool escaped,
      NodeLocation location)
    {
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Quoted quoted = new Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Quoted(value, contents, escaped);
      quoted.Location = location;
      return quoted;
    }

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Extend Extend(
      List<Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Selector> exact,
      List<Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Selector> partial,
      NodeLocation location)
    {
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Extend extend = new Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Extend(exact, partial);
      extend.Location = location;
      return extend;
    }

    public Node Attribute(Node key, Node op, Node val, NodeLocation location)
    {
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Attribute attribute = new Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Attribute(key, op, val);
      attribute.Location = location;
      return (Node) attribute;
    }

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Paren Paren(Node value, NodeLocation location)
    {
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Paren paren = new Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Paren(value);
      paren.Location = location;
      return paren;
    }

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Condition Condition(
      Node left,
      string operation,
      Node right,
      bool negate,
      NodeLocation location)
    {
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Condition condition = new Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Condition(left, operation, right, negate);
      condition.Location = location;
      return condition;
    }
  }
}
