// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Rule
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Exceptions;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes;
using Nop.Plugin.InstantSearch.Dotless.Plugins;
using System.Text.RegularExpressions;

namespace Nop.Plugin.InstantSearch.Dotless.Parser.Tree
{
  public class Rule : Node
  {
    public string Name { get; set; }

    public Node Value { get; set; }

    public bool Variable { get; set; }

    public NodeList PostNameComments { get; set; }

    public bool IsSemiColonRequired { get; set; }

    public bool Variadic { get; set; }

    public bool InterpolatedName { get; set; }

    public Rule(string name, Node value)
      : this(name, value, false)
    {
    }

    public Rule(string name, Node value, bool variadic)
    {
      this.Name = name;
      this.Value = value;
      this.Variable = !string.IsNullOrEmpty(name) && name[0] == '@';
      this.IsSemiColonRequired = true;
      this.Variadic = variadic;
    }

    public override Node Evaluate(Env env)
    {
      env.Rule = (Node) this;
      Rule rule = this.Value != null ? new Rule(this.EvaluateName(env), this.Value.Evaluate(env)).ReducedFrom<Rule>((Node) this) : throw new ParsingException("No value found for rule " + this.Name, this.Location);
      rule.IsSemiColonRequired = this.IsSemiColonRequired;
      rule.PostNameComments = this.PostNameComments;
      env.Rule = (Node) null;
      return (Node) rule;
    }

    private string EvaluateName(Env env)
    {
      if (!this.InterpolatedName)
        return this.Name;
      if (!(env.FindVariable(this.Name).Evaluate(env) is Rule rule))
        throw new ParsingException("Invalid variable value for property name", this.Location);
      if (!(rule.Value is Keyword keyword))
        throw new ParsingException("Invalid variable value for property name", this.Location);
      return keyword.ToCSS(env);
    }

    protected override Node CloneCore() => (Node) new Rule(this.Name, this.Value.Clone(), this.Variadic)
    {
      IsSemiColonRequired = this.IsSemiColonRequired,
      Variable = this.Variable
    };

    public override void AppendCSS(Env env)
    {
      if (this.Variable)
        return;
      Node node = this.Value;
      env.Output.Append(this.Name).Append((Node) this.PostNameComments).Append(env.Compress ? ":" : ": ");
      env.Output.Push().Append(node);
      if (env.Compress)
        env.Output.Reset(Regex.Replace(env.Output.ToString(), "(\\s)+", " ").Replace(", ", ","));
      env.Output.PopAndAppend();
      if (!this.IsSemiColonRequired)
        return;
      env.Output.Append(";");
    }

    public override void Accept(IVisitor visitor) => this.Value = this.VisitAndReplace<Node>(this.Value, visitor);
  }
}
