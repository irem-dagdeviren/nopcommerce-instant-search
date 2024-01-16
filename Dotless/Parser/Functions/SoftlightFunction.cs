// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Functions.SoftlightFunction
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

namespace Nop.Plugin.InstantSearch.Dotless.Parser.Functions
{
  public class SoftlightFunction : ColorMixFunction
  {
    protected override double Operate(double a, double b)
    {
      double num = b * a / (double) byte.MaxValue;
      return num + a * ((double) byte.MaxValue - ((double) byte.MaxValue - a) * ((double) byte.MaxValue - b) / (double) byte.MaxValue - num) / (double) byte.MaxValue;
    }
  }
}
