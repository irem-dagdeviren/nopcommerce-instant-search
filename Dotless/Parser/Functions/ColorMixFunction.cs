// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Functions.ColorMixFunction
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes;
using Nop.Plugin.InstantSearch.Dotless.Parser.Tree;
using Nop.Plugin.InstantSearch.Dotless.Utils;
using System;
using System.Collections.Generic;

namespace Nop.Plugin.InstantSearch.Dotless.Parser.Functions
{
  public abstract class ColorMixFunction : Function
  {
    protected override Node Evaluate(Env env)
    {
      Guard.ExpectNumArguments(2, this.Arguments.Count, (object) this, this.Location);
      Guard.ExpectAllNodes<Color>((IEnumerable<Node>) this.Arguments, (object) this, this.Location);
      Color backdrop = (Color) this.Arguments[0];
      Color source = (Color) this.Arguments[1];
      double num = source.Alpha + backdrop.Alpha * (1.0 - source.Alpha);
      return (Node) new Color(this.Compose(backdrop, source, num, (Func<Color, double>) (c => c.R)), this.Compose(backdrop, source, num, (Func<Color, double>) (c => c.G)), this.Compose(backdrop, source, num, (Func<Color, double>) (c => c.B)), num);
    }

    private double Compose(Color backdrop, Color source, double ar, Func<Color, double> channel)
    {
      double a = channel(backdrop);
      double b = channel(source);
      double alpha1 = backdrop.Alpha;
      double alpha2 = source.Alpha;
      double num = this.Operate(a, b);
      if (ar > 0.0)
        num = (alpha2 * b + alpha1 * (a - alpha2 * (a + b - num))) / ar;
      return num;
    }

    protected abstract double Operate(double a, double b);
  }
}
