// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Condition
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Exceptions;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes;
using Nop.Plugin.InstantSearch.Dotless.Plugins;
using System;

namespace Nop.Plugin.InstantSearch.Dotless.Parser.Tree
{
  public class Condition : Node
  {
    public Node Left { get; set; }

    public Node Right { get; set; }

    public string Operation { get; set; }

    public bool Negate { get; set; }

    public bool IsDefault { get; private set; }

    public Condition(Node left, string operation, Node right, bool negate)
    {
      this.Left = left;
      this.Right = right;
      this.Operation = operation.Trim();
      this.Negate = negate;
    }

    protected override Node CloneCore() => (Node) new Condition(this.Left.Clone(), this.Operation, this.Right.Clone(), this.Negate);

    public override void AppendCSS(Env env)
    {
    }

    public override Node Evaluate(Env env)
    {
      if (this.Left is Call left && left.Name == "default")
        this.IsDefault = true;
      bool flag = this.Evaluate(this.Left.Evaluate(env), this.Operation, this.Right.Evaluate(env));
      if (this.Negate)
        flag = !flag;
      return (Node) new BooleanNode(flag);
    }

    private bool Evaluate(Node lValue, string operation, Node rValue)
    {
      switch (operation)
      {
        case "or":
          return Condition.ToBool(lValue) || Condition.ToBool(rValue);
        case "and":
          return Condition.ToBool(lValue) && Condition.ToBool(rValue);
        default:
          int num;
          if (lValue is IComparable comparable1)
          {
            num = comparable1.CompareTo((object) rValue);
          }
          else
          {
            num = rValue is IComparable comparable ? comparable.CompareTo((object) lValue) : throw new ParsingException("Cannot compare objects in mixin guard condition", this.Location);
            if (num < 0)
              num = 1;
            else if (num > 0)
              num = -1;
          }
          if (num == 0)
            return operation == "=" || operation == ">=" || operation == "=<";
          if (num < 0)
            return operation == "<" || operation == "=<";
          if (num <= 0)
            throw new ParsingException("C# compiler can't work out it is impossible to get here", this.Location);
          return operation == ">" || operation == ">=";
      }
    }

    public bool Passes(Env env) => Condition.ToBool(this.Evaluate(env));

    private static bool ToBool(Node node) => node is BooleanNode booleanNode && booleanNode.Value;

    public override void Accept(IVisitor visitor)
    {
      this.Left = this.VisitAndReplace<Node>(this.Left, visitor);
      this.Right = this.VisitAndReplace<Node>(this.Right, visitor);
    }
  }
}
