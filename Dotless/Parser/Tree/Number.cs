// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Number
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes;
using Nop.Plugin.InstantSearch.Dotless.Utils;
using System;
using System.Globalization;

namespace Nop.Plugin.InstantSearch.Dotless.Parser.Tree
{
  public class Number : Node, IOperable, IComparable
  {
    private bool preferUnitFromSecondOperand;

    public double Value { get; set; }

    public string Unit { get; set; }

    public Number(string value, string unit)
    {
      this.Value = double.Parse(value, (IFormatProvider) CultureInfo.InvariantCulture);
      this.Unit = unit;
    }

    public Number(double value, string unit)
    {
      this.Value = value;
      this.Unit = unit;
    }

    public Number(double value)
      : this(value, "")
    {
    }

    private string FormatValue() => this.Value.ToString("0." + new string('#', this.GetPrecision()), (IFormatProvider) CultureInfo.InvariantCulture);

    private int GetPrecision() => 9;

    protected override Node CloneCore() => (Node) new Number(this.Value, this.Unit);

    public override void AppendCSS(Env env) => env.Output.Append(this.FormatValue()).Append(this.Unit);

    public Node Operate(Operation op, Node other)
    {
      Guard.ExpectNode<Number>(other, (object) ("right hand side of " + op.Operator), op.Location);
      Number number = (Number) other;
      string unit1 = this.Unit;
      string unit2 = number.Unit;
      if (this.preferUnitFromSecondOperand && !string.IsNullOrEmpty(unit2))
        unit1 = unit2;
      else if (string.IsNullOrEmpty(unit1))
        unit1 = unit2;
      else
        string.IsNullOrEmpty(unit2);
      return new Number(Operation.Operate(op.Operator, this.Value, number.Value), unit1)
      {
        preferUnitFromSecondOperand = (unit1 == unit2 && op.Operator == "/")
      }.ReducedFrom<Node>((Node) this, other);
    }

    public Color ToColor() => new Color(this.Value, this.Value, this.Value);

    public double ToNumber() => this.ToNumber(1.0);

    public double ToNumber(double max) => !(this.Unit == "%") ? this.Value : this.Value * max / 100.0;

    public static Number operator -(Number n) => new Number(-n.Value, n.Unit);

    public int CompareTo(object obj)
    {
      Number number = obj as Number;
      if (!(bool) (Node) number || number.Value > this.Value)
        return -1;
      return number.Value < this.Value ? 1 : 0;
    }
  }
}
