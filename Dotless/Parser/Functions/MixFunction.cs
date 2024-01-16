// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Functions.MixFunction
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes;
using Nop.Plugin.InstantSearch.Dotless.Parser.Tree;
using Nop.Plugin.InstantSearch.Dotless.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nop.Plugin.InstantSearch.Dotless.Parser.Functions
{
  public class MixFunction : Function
  {
    protected override Node Evaluate(Env env)
    {
      Guard.ExpectMinArguments(2, this.Arguments.Count, (object) this, this.Location);
      Guard.ExpectMaxArguments(3, this.Arguments.Count, (object) this, this.Location);
      Guard.ExpectAllNodes<Color>(this.Arguments.Take<Node>(2), (object) this, this.Location);
      double weight = 50.0;
      if (this.Arguments.Count == 3)
      {
        Guard.ExpectNode<Number>(this.Arguments[2], (object) this, this.Location);
        weight = ((Number) this.Arguments[2]).Value;
      }
      Color[] array = this.Arguments.Take<Node>(2).Cast<Color>().ToArray<Color>();
      return (Node) this.Mix(array[0], array[1], weight);
    }

    protected Color Mix(Color color1, Color color2, double weight)
    {
      double num1 = weight / 100.0;
      double num2 = num1 * 2.0 - 1.0;
      double num3 = color1.Alpha - color2.Alpha;
      double w1 = ((num2 * num3 == -1.0 ? num2 : (num2 + num3) / (1.0 + num2 * num3)) + 1.0) / 2.0;
      double w2 = 1.0 - w1;
      double[] array = ((IEnumerable<double>) color1.RGB).Select<double, double>((Func<double, int, double>) ((x, i) => x * w1 + color2.RGB[i] * w2)).ToArray<double>();
      double alpha = color1.Alpha * num1 + color2.Alpha * (1.0 - num1);
      return new Color(array[0], array[1], array[2], alpha);
    }
  }
}
