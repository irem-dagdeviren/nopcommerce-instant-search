// Decompiled with JetBrains decompiler
// Type:
// .Parser.Functions.DifferenceFunction
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using System;

namespace Nop.Plugin.InstantSearch.Dotless.Parser.Functions
{
  public class DifferenceFunction : ColorMixFunction
  {
    protected override double Operate(double a, double b) => Math.Abs(a - b);
  }
}
