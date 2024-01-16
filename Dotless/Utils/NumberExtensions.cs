// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Utils.NumberExtensions
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Parser.Tree;

namespace Nop.Plugin.InstantSearch.Dotless.Utils
{
  public static class NumberExtensions
  {
    public static double Normalize(this Number value) => value.Normalize(1.0);

    public static double Normalize(this Number value, double max) => value.Normalize(0.0, max);

    public static double Normalize(this Number value, double min, double max) => NumberExtensions.Normalize(value.ToNumber(max), min, max);

    public static double Normalize(double value) => NumberExtensions.Normalize(value, 1.0);

    public static double Normalize(double value, double max) => NumberExtensions.Normalize(value, 0.0, max);

    public static double Normalize(double value, double min, double max)
    {
      if (value < min)
        return min;
      return value <= max ? value : max;
    }
  }
}
