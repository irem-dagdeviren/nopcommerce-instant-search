// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Parsers
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Exceptions;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes;
using Nop.Plugin.InstantSearch.Dotless.Parser.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Nop.Plugin.InstantSearch.Dotless.Parser
{
  public class Parsers
  {
    private Stack<NodeList> CommentsStack = new Stack<NodeList>();
    private static readonly ImportOptions[][] illegalOptionCombinations = new ImportOptions[6][]
    {
      new ImportOptions[2]
      {
        ImportOptions.Css,
        ImportOptions.Less
      },
      new ImportOptions[2]
      {
        ImportOptions.Inline,
        ImportOptions.Css
      },
      new ImportOptions[2]
      {
        ImportOptions.Inline,
        ImportOptions.Less
      },
      new ImportOptions[2]
      {
        ImportOptions.Inline,
        ImportOptions.Reference
      },
      new ImportOptions[2]
      {
        ImportOptions.Once,
        ImportOptions.Multiple
      },
      new ImportOptions[2]
      {
        ImportOptions.Reference,
        ImportOptions.Css
      }
    };

    public INodeProvider NodeProvider { get; set; }

    public Parsers(INodeProvider nodeProvider) => this.NodeProvider = nodeProvider;

    public NodeList Primary(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser)
    {
      NodeList nodeList1 = new NodeList();
      this.GatherComments(parser);
      Node node;
      while ((bool) (node = (Node) this.MixinDefinition(parser) || (Node) this.ExtendRule(parser) || (Node) this.Rule(parser) || (Node) this.PullComments() || (Node) this.GuardedRuleset(parser) || (Node) this.Ruleset(parser) || (Node) this.MixinCall(parser) || this.Directive(parser)))
      {
        NodeList nodeList2;
        if ((bool) (Node) (nodeList2 = this.PullComments()))
          nodeList1.AddRange((IEnumerable<Node>) nodeList2);
        NodeList nodeList3 = node as NodeList;
        if ((bool) (Node) nodeList3)
        {
          foreach (Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Comment comment in (NodeList<Node>) nodeList3)
            comment.IsPreSelectorComment = true;
          nodeList1.AddRange((IEnumerable<Node>) nodeList3);
        }
        else
          nodeList1.Add(node);
        this.GatherComments(parser);
      }
      return nodeList1;
    }

    private NodeList CurrentComments { get; set; }

    private void GatherComments(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser)
    {
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Comment comment;
      while ((bool) (Node) (comment = this.Comment(parser)))
      {
        if (this.CurrentComments == null)
          this.CurrentComments = new NodeList();
        this.CurrentComments.Add((Node) comment);
      }
    }

    private NodeList PullComments()
    {
      NodeList currentComments = this.CurrentComments;
      this.CurrentComments = (NodeList) null;
      return currentComments;
    }

    private NodeList GatherAndPullComments(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser)
    {
      this.GatherComments(parser);
      return this.PullComments();
    }

    private void PushComments() => this.CommentsStack.Push(this.PullComments());

    private void PopComments() => this.CurrentComments = this.CommentsStack.Pop();

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Comment Comment(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser)
    {
      int index = parser.Tokenizer.Location.Index;
      string comment = parser.Tokenizer.GetComment();
      return comment != null ? this.NodeProvider.Comment(comment, parser.Tokenizer.GetNodeLocation(index)) : (Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Comment) null;
    }

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Quoted Quoted(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser)
    {
      int index = parser.Tokenizer.Location.Index;
      bool escaped = false;
      char ch = parser.Tokenizer.CurrentChar;
      if (parser.Tokenizer.CurrentChar == '~')
      {
        escaped = true;
        ch = parser.Tokenizer.NextChar;
      }
      if (ch != '"' && ch != '\'')
        return (Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Quoted) null;
      if (escaped)
        parser.Tokenizer.Match('~');
      string quotedString = parser.Tokenizer.GetQuotedString();
      return quotedString == null ? (Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Quoted) null : this.NodeProvider.Quoted(quotedString, quotedString.Substring(1, quotedString.Length - 2), escaped, parser.Tokenizer.GetNodeLocation(index));
    }

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Keyword Keyword(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser)
    {
      int index = parser.Tokenizer.Location.Index;
      RegexMatchResult regexMatchResult = parser.Tokenizer.Match("[A-Za-z0-9_-]+");
      return (bool) (Node) regexMatchResult ? this.NodeProvider.Keyword(regexMatchResult.Value, parser.Tokenizer.GetNodeLocation(index)) : (Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Keyword) null;
    }

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Call Call(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser)
    {
      Parsers.ParserLocation location = this.Remember(parser);
      int index = parser.Tokenizer.Location.Index;
      RegexMatchResult regexMatchResult = parser.Tokenizer.Match("(%|[a-zA-Z0-9_-]+|progid:[\\w\\.]+)\\(");
      if (!(Node) regexMatchResult)
        return (Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Call) null;
      if (regexMatchResult[1].ToLowerInvariant() == "alpha")
      {
        Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Alpha alpha = this.Alpha(parser);
        if (alpha != null)
          return (Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Call) alpha;
      }
      NodeList<Node> arguments = this.Arguments(parser);
      if (!!(Node) parser.Tokenizer.Match(')'))
        return this.NodeProvider.Call(regexMatchResult[1], arguments, parser.Tokenizer.GetNodeLocation(index));
      this.Recall(parser, location);
      return (Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Call) null;
    }

    public NodeList<Node> Arguments(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser)
    {
      NodeList<Node> nodeList = new NodeList<Node>();
      Node node;
      while ((bool) ((node = (Node) this.Assignment(parser)) || (node = (Node) this.Expression(parser))))
      {
        nodeList.Add(node);
        if (!(Node) parser.Tokenizer.Match(','))
          break;
      }
      return nodeList;
    }

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Assignment Assignment(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser)
    {
      RegexMatchResult regexMatchResult = parser.Tokenizer.Match("\\w+(?=\\s?=)");
      if (!(Node) regexMatchResult || !(Node) parser.Tokenizer.Match('='))
        return (Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Assignment) null;
      Node node = this.Entity(parser);
      return (bool) node ? this.NodeProvider.Assignment(regexMatchResult.Value, node, regexMatchResult.Location) : (Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Assignment) null;
    }

    public Node Literal(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser)
    {
      Node node = (Node) this.Dimension(parser) || (Node) this.Color(parser);
      return !(node ? true : false) ? node | (Node) this.Quoted(parser) : node;
    }

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Url Url(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser)
    {
      int index = parser.Tokenizer.Location.Index;
      if (parser.Tokenizer.CurrentChar != 'u' || !(Node) parser.Tokenizer.Match("url\\("))
        return (Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Url) null;
      this.GatherComments(parser);
      Node node = (Node) this.Quoted(parser);
      if (!node)
      {
        Parsers.ParserLocation location = this.Remember(parser);
        node = (Node) this.Expression(parser);
        if ((bool) node && !parser.Tokenizer.Peek(')'))
        {
          node = (Node) null;
          this.Recall(parser, location);
        }
      }
      else
      {
        node.PreComments = this.PullComments();
        node.PostComments = this.GatherAndPullComments(parser);
      }
      if (!node)
      {
        TextNode textNode = (TextNode) parser.Tokenizer.MatchAny("[^\\)\"']*");
        node = (Node) textNode ? (Node) textNode : (Node) (textNode | new TextNode(""));
      }
      this.Expect(parser, ')');
      return this.NodeProvider.Url(node, parser.Importer, parser.Tokenizer.GetNodeLocation(index));
    }

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Variable Variable(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser)
    {
      int index = parser.Tokenizer.Location.Index;
      RegexMatchResult regexMatchResult;
      return parser.Tokenizer.CurrentChar == '@' && (bool) (Node) (regexMatchResult = parser.Tokenizer.Match("@(@?[a-zA-Z0-9_-]+)")) ? this.NodeProvider.Variable(regexMatchResult.Value, parser.Tokenizer.GetNodeLocation(index)) : (Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Variable) null;
    }

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Variable InterpolatedVariable(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser)
    {
      int index = parser.Tokenizer.Location.Index;
      RegexMatchResult regexMatchResult;
      return parser.Tokenizer.CurrentChar == '@' && (bool) (Node) (regexMatchResult = parser.Tokenizer.Match("@\\{(?<name>@?[a-zA-Z0-9_-]+)\\}")) ? this.NodeProvider.Variable("@" + regexMatchResult.Match.Groups["name"].Value, parser.Tokenizer.GetNodeLocation(index)) : (Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Variable) null;
    }

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Variable VariableCurly(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser)
    {
      int index = parser.Tokenizer.Location.Index;
      RegexMatchResult regexMatchResult;
      return parser.Tokenizer.CurrentChar == '@' && (bool) (Node) (regexMatchResult = parser.Tokenizer.Match("@\\{([a-zA-Z0-9_-]+)\\}")) ? this.NodeProvider.Variable("@" + regexMatchResult.Match.Groups[1].Value, parser.Tokenizer.GetNodeLocation(index)) : (Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Variable) null;
    }

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.GuardedRuleset GuardedRuleset(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser)
    {
      NodeList<Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Selector> selectors = new NodeList<Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Selector>();
      Parsers.ParserLocation location = this.Remember(parser);
      int index = location.TokenizerLocation.Index;
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Selector selector;
      while ((bool) (Node) (selector = this.Selector(parser)))
      {
        selectors.Add(selector);
        if (!!(Node) parser.Tokenizer.Match(','))
          this.GatherComments(parser);
        else
          break;
      }
      if ((bool) (Node) parser.Tokenizer.Match("when"))
      {
        this.GatherAndPullComments(parser);
        Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Condition condition = this.Expect<Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Condition>(this.Conditions(parser), "Expected conditions after when (guard)", parser);
        NodeList rules = this.Block(parser);
        return this.NodeProvider.GuardedRuleset(selectors, rules, condition, parser.Tokenizer.GetNodeLocation(index));
      }
      this.Recall(parser, location);
      return (Nop.Plugin.InstantSearch.Dotless.Parser.Tree.GuardedRuleset) null;
    }

    public Extend ExtendRule(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser)
    {
      int index = parser.Tokenizer.Location.Index;
      RegexMatchResult regexMatchResult;
      if ((regexMatchResult = parser.Tokenizer.Match("\\&?:extend\\(")) == null)
        return (Extend) null;
      List<Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Selector> exact = new List<Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Selector>();
      List<Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Selector> partial = new List<Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Selector>();
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Selector selector;
      while ((bool) (Node) (selector = this.Selector(parser)))
      {
        if (selector.Elements.Count != 1 || selector.Elements.First<Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Element>().Value != null)
        {
          if (selector.Elements.Count > 1 && selector.Elements.Last<Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Element>().Value == "all")
          {
            selector.Elements.Remove(selector.Elements.Last<Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Element>());
            partial.Add(selector);
          }
          else
            exact.Add(selector);
          if (!(Node) parser.Tokenizer.Match(','))
            break;
        }
      }
      if (!(Node) parser.Tokenizer.Match(')'))
        throw new ParsingException("Extend rule not correctly terminated", parser.Tokenizer.GetNodeLocation(index));
      if (regexMatchResult.Match.Value[0] == '&')
        parser.Tokenizer.Match(';');
      return partial.Count == 0 && exact.Count == 0 ? (Extend) null : this.NodeProvider.Extend(exact, partial, parser.Tokenizer.GetNodeLocation(index));
    }

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Color Color(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser)
    {
      int index = parser.Tokenizer.Location.Index;
      RegexMatchResult regexMatchResult;
      return parser.Tokenizer.CurrentChar == '#' && (bool) (Node) (regexMatchResult = parser.Tokenizer.Match("#([a-fA-F0-9]{8}|[a-fA-F0-9]{6}|[a-fA-F0-9]{3})")) ? this.NodeProvider.Color(regexMatchResult[1], parser.Tokenizer.GetNodeLocation(index)) : (Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Color) null;
    }

    public Number Dimension(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser)
    {
      char currentChar = parser.Tokenizer.CurrentChar;
      if (!char.IsNumber(currentChar) && currentChar != '.' && currentChar != '-' && currentChar != '+')
        return (Number) null;
      int index = parser.Tokenizer.Location.Index;
      RegexMatchResult regexMatchResult = parser.Tokenizer.Match("([+-]?[0-9]*\\.?[0-9]+)(px|%|em|pc|ex|in|deg|s|ms|pt|cm|mm|ch|rem|vw|vh|vmin|vm(ax)?|grad|rad|fr|gr|Hz|kHz|dpi|dpcm|dppx)?", true);
      return (bool) (Node) regexMatchResult ? this.NodeProvider.Number(regexMatchResult[1], regexMatchResult[2], parser.Tokenizer.GetNodeLocation(index)) : (Number) null;
    }

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Script Script(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser)
    {
      if (parser.Tokenizer.CurrentChar != '`')
        return (Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Script) null;
      int index = parser.Tokenizer.Location.Index;
      RegexMatchResult regexMatchResult = parser.Tokenizer.MatchAny("`[^`]*`");
      return !(Node) regexMatchResult ? (Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Script) null : this.NodeProvider.Script(regexMatchResult.Value, parser.Tokenizer.GetNodeLocation(index));
    }

    public string VariableName(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser) => this.Variable(parser)?.Name;

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Shorthand Shorthand(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser)
    {
      if (!parser.Tokenizer.Peek("[@%\\w.-]+\\/[@%\\w.-]+"))
        return (Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Shorthand) null;
      int index = parser.Tokenizer.Location.Index;
      Node second = (Node) null;
      Node first;
      return (bool) ((first = this.Entity(parser)) && (Node) parser.Tokenizer.Match('/') && (second = this.Entity(parser))) ? this.NodeProvider.Shorthand(first, second, parser.Tokenizer.GetNodeLocation(index)) : (Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Shorthand) null;
    }

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.MixinCall MixinCall(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser)
    {
      NodeList<Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Element> elements = new NodeList<Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Element>();
      int index1 = parser.Tokenizer.Location.Index;
      bool important = false;
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Combinator combinator = (Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Combinator) null;
      this.PushComments();
      int index2 = parser.Tokenizer.Location.Index;
      RegexMatchResult regexMatchResult1;
      while ((bool) (Node) (regexMatchResult1 = parser.Tokenizer.Match("[#.][a-zA-Z0-9_-]+")))
      {
        elements.Add(this.NodeProvider.Element(combinator, (Node) regexMatchResult1, parser.Tokenizer.GetNodeLocation(index1)));
        int index3 = parser.Tokenizer.Location.Index;
        CharMatchResult charMatchResult = parser.Tokenizer.Match('>');
        combinator = charMatchResult != null ? this.NodeProvider.Combinator(charMatchResult.Value, parser.Tokenizer.GetNodeLocation(index1)) : (Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Combinator) null;
        int index4 = parser.Tokenizer.Location.Index;
      }
      if (elements.Count == 0)
      {
        this.PopComments();
        return (Nop.Plugin.InstantSearch.Dotless.Parser.Tree.MixinCall) null;
      }
      List<NamedArgument> arguments = new List<NamedArgument>();
      if (parser.Tokenizer.Peek('('))
      {
        Parsers.ParserLocation location = this.Remember(parser);
        RegexMatchResult regexMatchResult2 = parser.Tokenizer.Match("\\([^()]*(?>(?>(?'open'\\()[^()]*)*(?>(?'-open'\\))[^()]*)*)+(?(open)(?!))\\)");
        bool allowList = regexMatchResult2 != null && regexMatchResult2.Value.Contains<char>(';');
        char tok = allowList ? ';' : ',';
        this.Recall(parser, location);
        parser.Tokenizer.Match('(');
        Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Expression expression1;
        while ((bool) (Node) (expression1 = this.Expression(parser, allowList)))
        {
          Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Expression expression2 = expression1;
          string str = (string) null;
          if (expression1.Value.Count == 1 && expression1.Value[0] is Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Variable && (bool) (Node) parser.Tokenizer.Match(':'))
          {
            expression2 = this.Expect<Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Expression>(this.Expression(parser), "expected value", parser);
            str = (expression1.Value[0] as Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Variable).Name;
          }
          arguments.Add(new NamedArgument()
          {
            Name = str,
            Value = expression2
          });
          if (!(Node) parser.Tokenizer.Match(tok))
            break;
        }
        this.Expect(parser, ')');
      }
      this.GatherComments(parser);
      if (!string.IsNullOrEmpty(this.Important(parser)))
        important = true;
      NodeList nodeList = this.GatherAndPullComments(parser);
      if (this.End(parser))
      {
        Nop.Plugin.InstantSearch.Dotless.Parser.Tree.MixinCall mixinCall = this.NodeProvider.MixinCall(elements, arguments, important, parser.Tokenizer.GetNodeLocation(index1));
        mixinCall.PostComments = nodeList;
        this.PopComments();
        return mixinCall;
      }
      this.PopComments();
      return (Nop.Plugin.InstantSearch.Dotless.Parser.Tree.MixinCall) null;
    }

    private Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Expression Expression(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser, bool allowList) => !allowList ? this.Expression(parser) : this.ExpressionOrExpressionList(parser);

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.MixinDefinition MixinDefinition(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser)
    {
      if (parser.Tokenizer.CurrentChar != '.' && parser.Tokenizer.CurrentChar != '#' || parser.Tokenizer.Peek("[^{]*}"))
        return (Nop.Plugin.InstantSearch.Dotless.Parser.Tree.MixinDefinition) null;
      int index1 = parser.Tokenizer.Location.Index;
      Parsers.ParserLocation location = this.Remember(parser);
      RegexMatchResult regexMatchResult1 = parser.Tokenizer.Match("([#.](?:[\\w-]|\\\\(?:[a-fA-F0-9]{1,6} ?|[^a-fA-F0-9]))+)\\s*\\(");
      if (!(Node) regexMatchResult1)
        return (Nop.Plugin.InstantSearch.Dotless.Parser.Tree.MixinDefinition) null;
      this.PushComments();
      this.GatherAndPullComments(parser);
      string name = regexMatchResult1[1];
      bool variadic = false;
      NodeList<Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Rule> parameters = new NodeList<Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Rule>();
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Condition condition = (Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Condition) null;
      int index2;
      RegexMatchResult regexMatchResult2;
      while (true)
      {
        index2 = parser.Tokenizer.Location.Index;
        if (parser.Tokenizer.CurrentChar != '.' || !(bool) (Node) parser.Tokenizer.Match("\\.{3}"))
        {
          if ((bool) (Node) (regexMatchResult2 = parser.Tokenizer.Match("@[a-zA-Z0-9_-]+")))
          {
            this.GatherAndPullComments(parser);
            if ((bool) (Node) parser.Tokenizer.Match(':'))
            {
              this.GatherComments(parser);
              Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Expression expression = this.Expect<Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Expression>(this.Expression(parser), "Expected value", parser);
              parameters.Add(this.NodeProvider.Rule(regexMatchResult2.Value, (Node) expression, parser.Tokenizer.GetNodeLocation(index2)));
            }
            else if (!(bool) (Node) parser.Tokenizer.Match("\\.{3}"))
              parameters.Add(this.NodeProvider.Rule(regexMatchResult2.Value, (Node) null, parser.Tokenizer.GetNodeLocation(index2)));
            else
              goto label_11;
          }
          else
          {
            Node node;
            if ((bool) (node = this.Literal(parser) || (Node) this.Keyword(parser)))
              parameters.Add(this.NodeProvider.Rule((string) null, node, parser.Tokenizer.GetNodeLocation(index2)));
            else
              goto label_17;
          }
          this.GatherAndPullComments(parser);
          TextNode textNode = (TextNode) parser.Tokenizer.Match(',');
          if (!!((Node) textNode ? (Node) textNode : (Node) (textNode | (TextNode) parser.Tokenizer.Match(';'))))
            this.GatherAndPullComments(parser);
          else
            goto label_17;
        }
        else
          break;
      }
      variadic = true;
      goto label_17;
label_11:
      variadic = true;
      parameters.Add(this.NodeProvider.Rule(regexMatchResult2.Value, (Node) null, true, parser.Tokenizer.GetNodeLocation(index2)));
label_17:
      if (!(Node) parser.Tokenizer.Match(')'))
        this.Recall(parser, location);
      this.GatherAndPullComments(parser);
      if ((bool) (Node) parser.Tokenizer.Match("when"))
      {
        this.GatherAndPullComments(parser);
        condition = this.Expect<Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Condition>(this.Conditions(parser), "Expected conditions after when (mixin guards)", parser);
      }
      NodeList rules = this.Block(parser);
      this.PopComments();
      if (rules != null)
        return this.NodeProvider.MixinDefinition(name, parameters, rules, condition, variadic, parser.Tokenizer.GetNodeLocation(index1));
      this.Recall(parser, location);
      return (Nop.Plugin.InstantSearch.Dotless.Parser.Tree.MixinDefinition) null;
    }

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Condition Conditions(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser)
    {
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Condition left;
      if (!(bool) (Node) (left = this.Condition(parser)))
        return (Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Condition) null;
      while ((bool) (Node) parser.Tokenizer.Match(','))
      {
        Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Condition right = this.Expect<Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Condition>(this.Condition(parser), ", without recognised condition", parser);
        left = this.NodeProvider.Condition((Node) left, "or", (Node) right, false, parser.Tokenizer.GetNodeLocation());
      }
      return left;
    }

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Condition Condition(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser)
    {
      int index = parser.Tokenizer.Location.Index;
      bool negate = false;
      if ((bool) (Node) parser.Tokenizer.Match("not"))
        negate = true;
      this.Expect(parser, '(');
      Node left1 = this.Expect<Node>(this.Operation(parser) || (Node) this.Keyword(parser) || (Node) this.Quoted(parser), "unrecognised condition", parser);
      RegexMatchResult regexMatchResult = parser.Tokenizer.Match("(>=|=<|[<=>])");
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Condition left2;
      if ((bool) (Node) regexMatchResult)
      {
        Node right = this.Expect<Node>(this.Operation(parser) || (Node) this.Keyword(parser) || (Node) this.Quoted(parser), "unrecognised right hand side condition expression", parser);
        left2 = this.NodeProvider.Condition(left1, regexMatchResult.Value, right, negate, parser.Tokenizer.GetNodeLocation(index));
      }
      else
        left2 = this.NodeProvider.Condition(left1, "=", (Node) this.NodeProvider.Keyword("true", parser.Tokenizer.GetNodeLocation(index)), negate, parser.Tokenizer.GetNodeLocation(index));
      this.Expect(parser, ')');
      return (bool) (Node) parser.Tokenizer.Match("and") ? this.NodeProvider.Condition((Node) left2, "and", (Node) this.Condition(parser), false, parser.Tokenizer.GetNodeLocation(index)) : left2;
    }

    public Node Entity(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser)
    {
      Node node = this.Literal(parser) || (Node) this.Variable(parser) || (Node) this.Url(parser) || (Node) this.Call(parser) || (Node) this.Keyword(parser);
      return !(node ? true : false) ? node | (Node) this.Script(parser) : node;
    }

    private Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Expression ExpressionOrExpressionList(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser)
    {
      Parsers.ParserLocation location = this.Remember(parser);
      List<Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Expression> source = new List<Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Expression>();
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Expression expression;
      while ((bool) (Node) (expression = this.Expression(parser)))
      {
        source.Add(expression);
        if (!(Node) parser.Tokenizer.Match(','))
          break;
      }
      if (source.Count == 0)
      {
        this.Recall(parser, location);
        return (Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Expression) null;
      }
      return source.Count == 1 ? source[0] : new Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Expression(source.Cast<Node>(), true);
    }

    public bool End(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser) => (bool) (Node) parser.Tokenizer.Match(";[;\\s]*") || parser.Tokenizer.Peek('}');

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Alpha Alpha(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser)
    {
      int index = parser.Tokenizer.Location.Index;
      if (!(Node) parser.Tokenizer.Match("opacity\\s*=\\s*", true))
        return (Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Alpha) null;
      Node node;
      if (!(bool) (node = (Node) parser.Tokenizer.Match("[0-9]+") || (Node) this.Variable(parser)))
        return (Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Alpha) null;
      this.Expect(parser, ')');
      return this.NodeProvider.Alpha(node, parser.Tokenizer.GetNodeLocation(index));
    }

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Element Element(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser)
    {
      int index = parser.Tokenizer.Location.Index;
      this.GatherComments(parser);
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Combinator combinator = this.Combinator(parser);
      this.PushComments();
      this.GatherComments(parser);
      if (parser.Tokenizer.Peek("when"))
        return (Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Element) null;
      Node node = (Node) this.ExtendRule(parser) || this.NonPseudoClassSelector(parser) || (Node) Parsers.PseudoClassSelector(parser) || (Node) Parsers.PseudoElementSelector(parser) || (Node) parser.Tokenizer.Match('*') || (Node) parser.Tokenizer.Match('&') || this.Attribute(parser) || (Node) parser.Tokenizer.MatchAny("\\(((?<N>\\()|(?<-N>\\))|[^()@]*)+\\)") || (Node) parser.Tokenizer.Match("[\\.#](?=@\\{)") || (Node) this.VariableCurly(parser);
      if (!node && (bool) (Node) parser.Tokenizer.Match('('))
      {
        Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Variable variable = this.Variable(parser) ?? this.VariableCurly(parser);
        if ((bool) (Node) variable)
        {
          parser.Tokenizer.Match(')');
          node = (Node) this.NodeProvider.Paren((Node) variable, parser.Tokenizer.GetNodeLocation(index));
        }
      }
      if ((bool) node)
      {
        combinator.PostComments = this.PullComments();
        this.PopComments();
        combinator.PreComments = this.PullComments();
        return this.NodeProvider.Element(combinator, node, parser.Tokenizer.GetNodeLocation(index));
      }
      this.PopComments();
      return (Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Element) null;
    }

    private static RegexMatchResult PseudoClassSelector(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser) => parser.Tokenizer.Match(":(\\\\.|[a-zA-Z0-9_-])+");

    private static RegexMatchResult PseudoElementSelector(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser) => parser.Tokenizer.Match("::(\\\\.|[a-zA-Z0-9_-])+");

    private Node NonPseudoClassSelector(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser)
    {
      Parsers.ParserLocation location = this.Remember(parser);
      RegexMatchResult regexMatchResult = parser.Tokenizer.Match("[.#]?(\\\\.|[a-zA-Z0-9_-])+");
      if (!(Node) regexMatchResult)
        return (Node) null;
      if (!(bool) (Node) parser.Tokenizer.Match('('))
        return (Node) regexMatchResult;
      this.Recall(parser, location);
      return (Node) null;
    }

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Combinator Combinator(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser)
    {
      int index = parser.Tokenizer.Location.Index;
      Node node;
      return (bool) (node = (Node) parser.Tokenizer.Match("[+>~]")) ? this.NodeProvider.Combinator(node.ToString(), parser.Tokenizer.GetNodeLocation(index)) : this.NodeProvider.Combinator(char.IsWhiteSpace(parser.Tokenizer.GetPreviousCharIgnoringComments()) ? " " : (string) null, parser.Tokenizer.GetNodeLocation(index));
    }

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Selector Selector(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser)
    {
      int num = 0;
      NodeList<Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Element> elements1 = new NodeList<Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Element>();
      int index = parser.Tokenizer.Location.Index;
      this.GatherComments(parser);
      this.PushComments();
      if ((bool) (Node) parser.Tokenizer.Match('('))
      {
        Node node = this.Entity(parser);
        this.Expect(parser, ')');
        INodeProvider nodeProvider = this.NodeProvider;
        NodeList<Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Element> elements2 = new NodeList<Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Element>();
        elements2.Add(this.NodeProvider.Element((Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Combinator) null, node, parser.Tokenizer.GetNodeLocation(index)));
        NodeLocation nodeLocation = parser.Tokenizer.GetNodeLocation(index);
        return nodeProvider.Selector(elements2, nodeLocation);
      }
      while (true)
      {
        Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Element element = this.Element(parser);
        if (!!(Node) element)
        {
          ++num;
          elements1.Add(element);
        }
        else
          break;
      }
      if (num > 0)
      {
        Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Selector selector = this.NodeProvider.Selector(elements1, parser.Tokenizer.GetNodeLocation(index));
        selector.PostComments = this.GatherAndPullComments(parser);
        this.PopComments();
        selector.PreComments = this.PullComments();
        return selector;
      }
      this.PopComments();
      return (Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Selector) null;
    }

    public Node Tag(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser)
    {
      TextNode textNode = (TextNode) parser.Tokenizer.Match("[a-zA-Z][a-zA-Z-]*[0-9]?");
      return !((Node) textNode ? true : false) ? (Node) (textNode | (TextNode) parser.Tokenizer.Match('*')) : (Node) textNode;
    }

    public Node Attribute(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser)
    {
      int index = parser.Tokenizer.Location.Index;
      if (!(Node) parser.Tokenizer.Match('['))
        return (Node) null;
      Node key = (Node) this.InterpolatedVariable(parser) || (Node) parser.Tokenizer.Match("(\\\\.|[a-z0-9_-])+", true) || (Node) this.Quoted(parser);
      if (!key)
        return (Node) null;
      Node op = (Node) parser.Tokenizer.Match("[|~*$^]?=");
      TextNode textNode = (TextNode) this.Quoted(parser);
      Node val = (Node) textNode ? (Node) textNode : (Node) (textNode | (TextNode) parser.Tokenizer.Match("[\\w-]+"));
      this.Expect(parser, ']');
      return this.NodeProvider.Attribute(key, op, val, parser.Tokenizer.GetNodeLocation(index));
    }

    public NodeList Block(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser)
    {
      if (!(Node) parser.Tokenizer.Match('{'))
        return (NodeList) null;
      NodeList nodeList = this.Expect<NodeList>(this.Primary(parser), "Expected content inside block", parser);
      this.Expect(parser, '}');
      return nodeList;
    }

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Ruleset Ruleset(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser)
    {
      NodeList<Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Selector> selectors = new NodeList<Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Selector>();
      Parsers.ParserLocation location = this.Remember(parser);
      int index = location.TokenizerLocation.Index;
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Selector selector;
      while ((bool) (Node) (selector = this.Selector(parser)))
      {
        selectors.Add(selector);
        if (!!(Node) parser.Tokenizer.Match(','))
          this.GatherComments(parser);
        else
          break;
      }
      NodeList rules;
      if (selectors.Count > 0 && (rules = this.Block(parser)) != null)
        return this.NodeProvider.Ruleset(selectors, rules, parser.Tokenizer.GetNodeLocation(index));
      this.Recall(parser, location);
      return (Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Ruleset) null;
    }

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Rule Rule(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser)
    {
      Parsers.ParserLocation location = this.Remember(parser);
      this.PushComments();
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Variable variable1 = (Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Variable) null;
      string str = this.Property(parser);
      bool flag = false;
      if (string.IsNullOrEmpty(str))
      {
        variable1 = this.Variable(parser);
        if (variable1 != null)
        {
          str = variable1.Name;
        }
        else
        {
          Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Variable variable2 = this.InterpolatedVariable(parser);
          if (variable2 != null)
          {
            flag = true;
            str = variable2.Name;
          }
        }
      }
      NodeList nodeList1 = this.GatherAndPullComments(parser);
      if (str != null && (bool) (Node) parser.Tokenizer.Match(':'))
      {
        NodeList nodeList2 = this.GatherAndPullComments(parser);
        Node node = !(str == "font") ? (!this.MatchesProperty("filter", str) ? (Node) this.Value(parser) : (Node) this.FilterExpressionList(parser) || (Node) this.Value(parser)) : (Node) this.Font(parser);
        if (variable1 != null && node == null)
          node = (Node) parser.Tokenizer.Match("[^;]*");
        NodeList nodeList3 = this.GatherAndPullComments(parser);
        if (this.End(parser))
        {
          if (node == null)
            throw new ParsingException(str + " is incomplete", parser.Tokenizer.GetNodeLocation());
          node.PreComments = nodeList2;
          node.PostComments = nodeList3;
          Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Rule rule = this.NodeProvider.Rule(str, node, parser.Tokenizer.GetNodeLocation(location.TokenizerLocation.Index));
          if (flag)
          {
            rule.InterpolatedName = true;
            rule.Variable = false;
          }
          rule.PostNameComments = nodeList1;
          this.PopComments();
          return rule;
        }
      }
      this.PopComments();
      this.Recall(parser, location);
      return (Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Rule) null;
    }

    private bool MatchesProperty(string expectedPropertyName, string actualPropertyName) => string.Equals(expectedPropertyName, actualPropertyName) || Regex.IsMatch(actualPropertyName, string.Format("-(\\w+)-{0}", (object) expectedPropertyName));

    private CssFunctionList FilterExpressionList(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser)
    {
      CssFunctionList source = new CssFunctionList();
      Node node;
      while ((bool) (node = this.FilterExpression(parser)))
        source.Add(node);
      return !source.Any<Node>() ? (CssFunctionList) null : source;
    }

    private Node FilterExpression(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser)
    {
      int index = parser.Tokenizer.Location.Index;
      this.GatherComments(parser);
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Url url = this.Url(parser);
      if ((bool) (Node) url)
        return (Node) url;
      RegexMatchResult regexMatchResult = parser.Tokenizer.Match("\\s*(blur|brightness|contrast|drop-shadow|grayscale|hue-rotate|invert|opacity|saturate|sepia|url)\\s*\\(");
      if (regexMatchResult == null)
        return (Node) null;
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Value obj = this.Value(parser);
      if (obj == null)
        return (Node) null;
      this.Expect(parser, ')');
      CssFunction cssFunction = this.NodeProvider.CssFunction(regexMatchResult.Match.Groups[1].Value.Trim(), (Node) obj, parser.Tokenizer.GetNodeLocation(index));
      cssFunction.PreComments = this.PullComments();
      cssFunction.PostComments = this.GatherAndPullComments(parser);
      return (Node) cssFunction;
    }

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Import Import(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser)
    {
      int index = parser.Tokenizer.Location.Index;
      if (!(Node) parser.Tokenizer.Match("@import(-(once))?\\s+"))
        return (Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Import) null;
      ImportOptions options = Parsers.ParseOptions(parser);
      Node path = (Node) this.Quoted(parser) || (Node) this.Url(parser);
      if (!path)
        return (Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Import) null;
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Value features = this.MediaFeatures(parser);
      this.Expect(parser, ';', "Expected ';' (possibly unrecognised media sequence)");
      switch (path)
      {
        case Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Quoted _:
          return this.NodeProvider.Import(path as Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Quoted, features, options, parser.Tokenizer.GetNodeLocation(index));
        case Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Url _:
          return this.NodeProvider.Import(path as Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Url, features, options, parser.Tokenizer.GetNodeLocation(index));
        default:
          throw new ParsingException("unrecognised @import format", parser.Tokenizer.GetNodeLocation(index));
      }
    }

    private static ImportOptions ParseOptions(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser)
    {
      int index = parser.Tokenizer.Location.Index;
      RegexMatchResult regexMatchResult = parser.Tokenizer.Match("\\((?<keywords>.*)\\)");
      if (!(Node) regexMatchResult)
        return ImportOptions.Once;
      string allKeywords = regexMatchResult.Match.Groups["keywords"].Value;
      IEnumerable<string> strings = ((IEnumerable<string>) allKeywords.Split(',')).Select<string, string>((Func<string, string>) (kw => kw.Trim()));
      ImportOptions options = (ImportOptions) 0;
      foreach (string str in strings)
      {
        try
        {
          ImportOptions importOptions = (ImportOptions) Enum.Parse(typeof (ImportOptions), str, true);
          options |= importOptions;
        }
        catch (ArgumentException ex)
        {
          throw new ParsingException(string.Format("unrecognized @import option '{0}'", (object) str), parser.Tokenizer.GetNodeLocation(index));
        }
      }
      Parsers.CheckForConflictingOptions(parser, options, allKeywords, index);
      return options;
    }

    private static void CheckForConflictingOptions(
      Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser,
      ImportOptions options,
      string allKeywords,
      int index)
    {
      foreach (ImportOptions[] optionCombination in Parsers.illegalOptionCombinations)
      {
        if (Parsers.IsOptionSet(options, optionCombination[0]) && Parsers.IsOptionSet(options, optionCombination[1]))
          throw new ParsingException(string.Format("invalid combination of @import options ({0}) -- specify either {1} or {2}, but not both", (object) allKeywords, (object) optionCombination[0].ToString().ToLowerInvariant(), (object) optionCombination[1].ToString().ToLowerInvariant()), parser.Tokenizer.GetNodeLocation(index));
      }
    }

    private static bool IsOptionSet(ImportOptions options, ImportOptions test) => (options & test) == test;

    public Node Directive(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser)
    {
      if (parser.Tokenizer.CurrentChar != '@')
        return (Node) null;
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Import import = this.Import(parser);
      if ((bool) (Node) import)
        return (Node) import;
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Media media = this.Media(parser);
      if ((bool) (Node) media)
        return (Node) media;
      this.GatherComments(parser);
      int index = parser.Tokenizer.Location.Index;
      string name = parser.Tokenizer.MatchString("@[-a-z]+");
      if (string.IsNullOrEmpty(name))
        return (Node) null;
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = false;
      string tok = "[^{]+";
      string str = name;
      if (name.StartsWith("@-") && name.IndexOf('-', 2) > 0)
        str = "@" + name.Substring(name.IndexOf('-', 2) + 1);
      switch (str)
      {
        case "@bottom-center":
        case "@bottom-left":
        case "@bottom-left-corner":
        case "@bottom-right":
        case "@bottom-right-corner":
        case "@left-bottom":
        case "@left-middle":
        case "@left-top":
        case "@right-bottom":
        case "@right-middle":
        case "@right-top":
        case "@top-center":
        case "@top-left":
        case "@top-left-corner":
        case "@top-right":
        case "@top-right-corner":
        case "@viewport":
          flag2 = true;
          break;
        case "@document":
        case "@page":
        case "@supports":
          flag2 = true;
          flag1 = true;
          break;
        case "@font-face":
          flag2 = true;
          break;
        case "@keyframes":
          flag3 = true;
          flag1 = true;
          break;
      }
      string identifier = "";
      NodeList nodeList1 = this.PullComments();
      if (flag1)
      {
        this.GatherComments(parser);
        RegexMatchResult regexMatchResult = parser.Tokenizer.MatchAny(tok);
        if (regexMatchResult != null)
          identifier = regexMatchResult.Value.Trim();
      }
      NodeList nodeList2 = this.GatherAndPullComments(parser);
      if (flag2)
      {
        NodeList rules = this.Block(parser);
        if (rules != null)
        {
          rules.PreComments = nodeList2;
          return (Node) this.NodeProvider.Directive(name, identifier, rules, parser.Tokenizer.GetNodeLocation(index));
        }
      }
      else
      {
        if (flag3)
        {
          Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Directive directive = this.KeyFrameBlock(parser, name, identifier, index);
          directive.PreComments = nodeList2;
          return (Node) directive;
        }
        Node node;
        if ((bool) (node = (Node) this.Expression(parser)))
        {
          node.PreComments = nodeList2;
          node.PostComments = this.GatherAndPullComments(parser);
          this.Expect(parser, ';', "missing semicolon in expression");
          Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Directive directive = this.NodeProvider.Directive(name, node, parser.Tokenizer.GetNodeLocation(index));
          directive.PreComments = nodeList1;
          return (Node) directive;
        }
      }
      throw new ParsingException("directive block with unrecognised format", parser.Tokenizer.GetNodeLocation(index));
    }

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Expression MediaFeature(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser)
    {
      NodeList expression = new NodeList();
      int index1 = parser.Tokenizer.Location.Index;
      while (true)
      {
        this.GatherComments(parser);
        Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Keyword keyword = this.Keyword(parser);
        if ((bool) (Node) keyword)
        {
          keyword.PreComments = this.PullComments();
          keyword.PostComments = this.GatherAndPullComments(parser);
          expression.Add((Node) keyword);
        }
        else if ((bool) (Node) parser.Tokenizer.Match('('))
        {
          this.GatherComments(parser);
          Parsers.ParserLocation location1 = this.Remember(parser);
          int index2 = parser.Tokenizer.Location.Index;
          string name = this.Property(parser);
          this.GatherAndPullComments(parser);
          if (!string.IsNullOrEmpty(name) && !(Node) parser.Tokenizer.Match(':'))
          {
            this.Recall(parser, location1);
            name = (string) null;
          }
          this.GatherComments(parser);
          Parsers.ParserLocation location2 = this.Remember(parser);
          Node node = this.Entity(parser);
          if (!node || !(Node) parser.Tokenizer.Match(')'))
          {
            this.Recall(parser, location2);
            RegexMatchResult regexMatchResult = parser.Tokenizer.Match("[^\\){]+");
            if ((bool) (Node) regexMatchResult)
            {
              node = (Node) this.NodeProvider.TextNode(regexMatchResult.Value, parser.Tokenizer.GetNodeLocation());
              this.Expect(parser, ')');
            }
          }
          if (!!node)
          {
            node.PreComments = this.PullComments();
            node.PostComments = this.GatherAndPullComments(parser);
            if (!string.IsNullOrEmpty(name))
            {
              Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Rule rule = this.NodeProvider.Rule(name, node, parser.Tokenizer.GetNodeLocation(index2));
              rule.IsSemiColonRequired = false;
              expression.Add((Node) this.NodeProvider.Paren((Node) rule, parser.Tokenizer.GetNodeLocation(index2)));
            }
            else
              expression.Add((Node) this.NodeProvider.Paren(node, parser.Tokenizer.GetNodeLocation(index2)));
          }
          else
            break;
        }
        else
          goto label_14;
      }
      return (Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Expression) null;
label_14:
      return expression.Count == 0 ? (Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Expression) null : this.NodeProvider.Expression(expression, parser.Tokenizer.GetNodeLocation(index1));
    }

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Value MediaFeatures(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser)
    {
      List<Node> values = new List<Node>();
      int index = parser.Tokenizer.Location.Index;
      do
      {
        Node node = (Node) this.MediaFeature(parser) || (Node) this.Variable(parser);
        if (!node)
          return (Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Value) null;
        values.Add(node);
      }
      while (!!(Node) parser.Tokenizer.Match(","));
      return this.NodeProvider.Value((IEnumerable<Node>) values, (string) null, parser.Tokenizer.GetNodeLocation(index));
    }

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Media Media(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser)
    {
      if (!(Node) parser.Tokenizer.Match("@media"))
        return (Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Media) null;
      int index = parser.Tokenizer.Location.Index;
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Value features = this.MediaFeatures(parser);
      NodeList nodeList = this.GatherAndPullComments(parser);
      NodeList rules = this.Expect<NodeList>(this.Block(parser), "@media block with unrecognised format", parser);
      rules.PreComments = nodeList;
      return this.NodeProvider.Media(rules, features, parser.Tokenizer.GetNodeLocation(index));
    }

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Directive KeyFrameBlock(
      Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser,
      string name,
      string identifier,
      int index)
    {
      if (!(Node) parser.Tokenizer.Match('{'))
        return (Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Directive) null;
      NodeList rules1 = new NodeList();
      while (true)
      {
        this.GatherComments(parser);
        NodeList identifier1 = new NodeList();
        while (true)
        {
          RegexMatchResult regexMatchResult;
          if (identifier1.Count > 0)
          {
            regexMatchResult = this.Expect<RegexMatchResult>(parser.Tokenizer.Match("from|to|([0-9\\.]+%)"), "@keyframe block unknown identifier", parser);
          }
          else
          {
            regexMatchResult = parser.Tokenizer.Match("from|to|([0-9\\.]+%)");
            if (!(Node) regexMatchResult)
              break;
          }
          identifier1.Add((Node) new Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Element((Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Combinator) null, (Node) regexMatchResult));
          this.GatherComments(parser);
          if (!!(Node) parser.Tokenizer.Match(","))
            this.GatherComments(parser);
          else
            break;
        }
        if (identifier1.Count != 0)
        {
          NodeList nodeList = this.GatherAndPullComments(parser);
          NodeList rules2 = this.Expect<NodeList>(this.Block(parser), "Expected css block after key frame identifier", parser);
          rules2.PreComments = nodeList;
          rules2.PostComments = this.GatherAndPullComments(parser);
          rules1.Add((Node) this.NodeProvider.KeyFrame(identifier1, rules2, parser.Tokenizer.GetNodeLocation()));
        }
        else
          break;
      }
      this.Expect(parser, '}', "Expected start, finish, % or '}}' but got {1}");
      return this.NodeProvider.Directive(name, identifier, rules1, parser.Tokenizer.GetNodeLocation(index));
    }

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Value Font(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser)
    {
      NodeList values = new NodeList();
      NodeList expression = new NodeList();
      int index = parser.Tokenizer.Location.Index;
      Node node1;
      while ((bool) (node1 = (Node) this.Shorthand(parser) || this.Entity(parser)))
        expression.Add(node1);
      values.Add((Node) this.NodeProvider.Expression(expression, parser.Tokenizer.GetNodeLocation(index)));
      if ((bool) (Node) parser.Tokenizer.Match(','))
      {
        Node node2;
        while ((bool) (node2 = (Node) this.Expression(parser)))
        {
          values.Add(node2);
          if (!(Node) parser.Tokenizer.Match(','))
            break;
        }
      }
      return this.NodeProvider.Value((IEnumerable<Node>) values, this.Important(parser), parser.Tokenizer.GetNodeLocation(index));
    }

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Value Value(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser)
    {
      NodeList values = new NodeList();
      int index = parser.Tokenizer.Location.Index;
      Node node;
      while ((bool) (node = (Node) this.Expression(parser)))
      {
        values.Add(node);
        if (!(Node) parser.Tokenizer.Match(','))
          break;
      }
      this.GatherComments(parser);
      string important = string.Join(" ", ((IEnumerable<string>) new string[2]
      {
        this.IESlash9Hack(parser),
        this.Important(parser)
      }).Where<string>((Func<string, bool>) (x => x != "")).ToArray<string>());
      if (values.Count <= 0 && !(bool) (Node) parser.Tokenizer.Match(';'))
        return (Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Value) null;
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Value obj = this.NodeProvider.Value((IEnumerable<Node>) values, important, parser.Tokenizer.GetNodeLocation(index));
      if (!string.IsNullOrEmpty(important))
        obj.PreImportantComments = this.PullComments();
      return obj;
    }

    public string Important(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser)
    {
      RegexMatchResult regexMatchResult = parser.Tokenizer.Match("!\\s*important");
      return regexMatchResult != null ? regexMatchResult.Value : "";
    }

    public string IESlash9Hack(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser)
    {
      RegexMatchResult regexMatchResult = parser.Tokenizer.Match("\\\\9");
      return regexMatchResult != null ? regexMatchResult.Value : "";
    }

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Expression Sub(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser)
    {
      if (!(Node) parser.Tokenizer.Match('('))
        return (Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Expression) null;
      Parsers.ParserLocation location = this.Remember(parser);
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Expression expression = this.Expression(parser);
      if (expression != null && (bool) (Node) parser.Tokenizer.Match(')'))
        return expression;
      this.Recall(parser, location);
      return (Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Expression) null;
    }

    public Node Multiplication(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser)
    {
      this.GatherComments(parser);
      Node node = this.Operand(parser);
      if (!node)
        return (Node) null;
      Node left = node;
      while (true)
      {
        this.GatherComments(parser);
        int index = parser.Tokenizer.Location.Index;
        RegexMatchResult regexMatchResult = parser.Tokenizer.Match("[\\/*]");
        this.GatherComments(parser);
        Node right = (Node) null;
        if ((bool) ((Node) regexMatchResult && (right = this.Operand(parser))))
          left = (Node) this.NodeProvider.Operation(regexMatchResult.Value, left, right, parser.Tokenizer.GetNodeLocation(index));
        else
          break;
      }
      return left;
    }

    public Node UnicodeRange(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser) => (Node) parser.Tokenizer.Match("(U\\+[0-9a-f]+(-[0-9a-f]+))", true) ?? (Node) parser.Tokenizer.Match("(U\\+[0-9a-f?]+)", true);

    public Node Operation(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser)
    {
      bool strictMath = parser.StrictMath;
      try
      {
        parser.StrictMath = false;
        if (strictMath && parser.Tokenizer.Match('(') == null)
          return (Node) null;
        Node node = this.Multiplication(parser);
        if (!node)
          return (Node) null;
        Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Operation operation = (Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Operation) null;
        while (true)
        {
          this.GatherComments(parser);
          int index = parser.Tokenizer.Location.Index;
          RegexMatchResult regexMatchResult = parser.Tokenizer.Match("[-+]\\s+");
          if (!(Node) regexMatchResult && !char.IsWhiteSpace(parser.Tokenizer.GetPreviousCharIgnoringComments()))
            regexMatchResult = parser.Tokenizer.Match("[-+]");
          Node right = (Node) null;
          if ((bool) ((Node) regexMatchResult && (right = this.Multiplication(parser))))
            operation = this.NodeProvider.Operation(regexMatchResult.Value, (Node) operation ?? node, right, parser.Tokenizer.GetNodeLocation(index));
          else
            break;
        }
        if (strictMath)
          this.Expect(parser, ')', "Missing closing paren.");
        return (Node) operation ?? node;
      }
      finally
      {
        parser.StrictMath = strictMath;
      }
    }

    public Node Operand(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser)
    {
      CharMatchResult charMatchResult = (CharMatchResult) null;
      if (parser.Tokenizer.CurrentChar == '-' && parser.Tokenizer.Peek("-[@\\(]"))
      {
        charMatchResult = parser.Tokenizer.Match('-');
        this.GatherComments(parser);
      }
      Node node1 = (Node) this.Sub(parser);
      if (node1 == null)
      {
        Number number = this.Dimension(parser);
        if (number == null)
        {
          Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Color color = this.Color(parser);
          node1 = color != null ? (Node) color : (Node) this.Variable(parser);
        }
        else
          node1 = (Node) number;
      }
      Node right = node1;
      if (right != null)
        return !(bool) (Node) charMatchResult ? right : (Node) this.NodeProvider.Operation("*", (Node) this.NodeProvider.Number("-1", "", charMatchResult.Location), right, charMatchResult.Location);
      if (parser.Tokenizer.CurrentChar == 'u' && parser.Tokenizer.Peek("url\\("))
        return (Node) null;
      Node node2 = (Node) this.Call(parser);
      return !(node2 ? true : false) ? node2 | (Node) this.Keyword(parser) : node2;
    }

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Expression Expression(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser)
    {
      NodeList expression = new NodeList();
      int index = parser.Tokenizer.Location.Index;
      Node node;
      while ((bool) (node = this.UnicodeRange(parser) || this.Operation(parser) || this.Entity(parser) || (Node) parser.Tokenizer.Match("[-+*/]")))
      {
        node.PostComments = this.PullComments();
        expression.Add(node);
      }
      return expression.Count > 0 ? this.NodeProvider.Expression(expression, parser.Tokenizer.GetNodeLocation(index)) : (Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Expression) null;
    }

    public string Property(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser)
    {
      RegexMatchResult regexMatchResult = parser.Tokenizer.Match("\\*?-?[-_a-zA-Z][-_a-z0-9A-Z]*");
      return (bool) (Node) regexMatchResult ? regexMatchResult.Value : (string) null;
    }

    public void Expect(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser, char expectedString) => this.Expect(parser, expectedString, (string) null);

    public void Expect(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser, char expectedString, string message)
    {
      if (!(bool) (Node) parser.Tokenizer.Match(expectedString))
      {
        message = message ?? "Expected '{0}' but found '{1}'";
        throw new ParsingException(string.Format(message, (object) expectedString, (object) parser.Tokenizer.NextChar), parser.Tokenizer.GetNodeLocation());
      }
    }

    public T Expect<T>(T node, string message, Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser) where T : Node => (bool) (Node) node ? node : throw new ParsingException(message, parser.Tokenizer.GetNodeLocation());

    public Parsers.ParserLocation Remember(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser) => new Parsers.ParserLocation()
    {
      Comments = this.CurrentComments,
      TokenizerLocation = parser.Tokenizer.Location
    };

    public void Recall(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser, Parsers.ParserLocation location)
    {
      this.CurrentComments = location.Comments;
      parser.Tokenizer.Location = location.TokenizerLocation;
    }

    public class ParserLocation
    {
      public NodeList Comments { get; set; }

      public Location TokenizerLocation { get; set; }
    }
  }
}
