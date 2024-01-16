// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Operation
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
  public class Operation : Node
  {
    public Node First { get; set; }

    public Node Second { get; set; }

    public string Operator { get; set; }

    public Operation(string op, Node first, Node second)
    {
      this.First = first;
      this.Second = second;
      this.Operator = op.Trim();
    }

    protected override Node CloneCore() => (Node) new Operation(this.Operator, this.First.Clone(), this.Second.Clone());

    public override Node Evaluate(Env env)
    {
      Node node1 = this.First.Evaluate(env);
      Node other = this.Second.Evaluate(env);
      if (node1 is Number && other is Color)
      {
        if (!(this.Operator == "*") && !(this.Operator == "+"))
          throw new ParsingException("Can't substract or divide a color from a number", this.Location);
        Node node2 = other;
        other = node1;
        node1 = node2;
      }
      try
      {
        return node1 is IOperable operable ? operable.Operate(this, other).ReducedFrom<Node>((Node) this) : throw new ParsingException(string.Format("Cannot apply operator {0} to the left hand side: {1}", (object) this.Operator, (object) node1.ToCSS(env)), this.Location);
      }
      catch (DivideByZeroException ex)
      {
        NodeLocation location = this.Location;
        throw new ParsingException((Exception) ex, location);
      }
      catch (InvalidOperationException ex)
      {
        NodeLocation location = this.Location;
        throw new ParsingException((Exception) ex, location);
      }
    }

    public static double Operate(string op, double first, double second)
    {
      if (op == "/" && second == 0.0)
        throw new DivideByZeroException();
      switch (op)
      {
        case "+":
          return first + second;
        case "-":
          return first - second;
        case "*":
          return first * second;
        case "/":
          return first / second;
        default:
          throw new InvalidOperationException("Unknown operator");
      }
    }

    public override void Accept(IVisitor visitor)
    {
      this.First = this.VisitAndReplace<Node>(this.First, visitor);
      this.Second = this.VisitAndReplace<Node>(this.Second, visitor);
    }
  }
}
