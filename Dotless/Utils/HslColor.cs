// Decompiled with JetBrains decompiler
// Type:
// .Utils.HslColor
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Parser.Tree;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nop.Plugin.InstantSearch.Dotless.Utils
{
  public class HslColor
  {
    private double _hue;
    private double _saturation;
    private double _lightness;

    public double Hue
    {
      get => this._hue;
      set => this._hue = value % 1.0;
    }

    public double Saturation
    {
      get => this._saturation;
      set => this._saturation = NumberExtensions.Normalize(value);
    }

    public double Lightness
    {
      get => this._lightness;
      set => this._lightness = NumberExtensions.Normalize(value);
    }

    public double Alpha { get; set; }

    public HslColor(double hue, double saturation, double lightness)
      : this(hue, saturation, lightness, 1.0)
    {
    }

    public HslColor(double hue, double saturation, double lightness, double alpha)
    {
      this.Hue = hue;
      this.Saturation = saturation;
      this.Lightness = lightness;
      this.Alpha = alpha;
    }

    public HslColor(Number hue, Number saturation, Number lightness, Number alpha)
    {
      this.Hue = hue.ToNumber() / 360.0 % 1.0;
      this.Saturation = saturation.Normalize(100.0) / 100.0;
      this.Lightness = lightness.Normalize(100.0) / 100.0;
      this.Alpha = alpha.Normalize();
    }

    public static HslColor FromRgbColor(Color color)
    {
      double[] array1 = ((IEnumerable<double>) color.RGB).Select<double, double>((Func<double, double>) (x => x / (double) byte.MaxValue)).ToArray<double>();
      double num = ((IEnumerable<double>) array1).Min();
      double max = ((IEnumerable<double>) array1).Max();
      double range = max - num;
      double lightness = (max + num) / 2.0;
      double saturation = 0.0;
      double hue = 0.0;
      if (range != 0.0)
      {
        saturation = lightness >= 0.5 ? range / (2.0 - max - num) : range / (max + num);
        double[] array2 = ((IEnumerable<double>) array1).Select<double, double>((Func<double, double>) (x => ((max - x) / 6.0 + range / 2.0) / range)).ToArray<double>();
        if (array1[0] == max)
          hue = array2[2] - array2[1];
        else if (array1[1] == max)
          hue = 1.0 / 3.0 + array2[0] - array2[2];
        else if (array1[2] == max)
          hue = 2.0 / 3.0 + array2[1] - array2[0];
        if (hue < 0.0)
          ++hue;
        if (hue > 1.0)
          --hue;
      }
      return new HslColor(hue, saturation, lightness, color.Alpha);
    }

    public Color ToRgbColor()
    {
      if (this.Saturation == 0.0)
      {
        double num = Math.Round(this.Lightness * (double) byte.MaxValue);
        return new Color(num, num, num, this.Alpha);
      }
      double v2 = this.Lightness >= 0.5 ? this.Lightness + this.Saturation - this.Saturation * this.Lightness : this.Lightness * (1.0 + this.Saturation);
      double v1 = 2.0 * this.Lightness - v2;
      double r = (double) byte.MaxValue * HslColor.Hue_2_RGB(v1, v2, this.Hue + 1.0 / 3.0);
      double num1 = (double) byte.MaxValue * HslColor.Hue_2_RGB(v1, v2, this.Hue);
      double num2 = (double) byte.MaxValue * HslColor.Hue_2_RGB(v1, v2, this.Hue - 1.0 / 3.0);
      double g = num1;
      double b = num2;
      double alpha = this.Alpha;
      return new Color(r, g, b, alpha);
    }

    private static double Hue_2_RGB(double v1, double v2, double vH)
    {
      if (vH < 0.0)
        ++vH;
      if (vH > 1.0)
        --vH;
      if (6.0 * vH < 1.0)
        return v1 + (v2 - v1) * 6.0 * vH;
      if (2.0 * vH < 1.0)
        return v2;
      return 3.0 * vH < 2.0 ? v1 + (v2 - v1) * (2.0 / 3.0 - vH) * 6.0 : v1;
    }

    public Number GetHueInDegrees() => new Number(this.Hue * 360.0, "deg");

    public Number GetSaturation() => new Number(this.Saturation * 100.0, "%");

    public Number GetLightness() => new Number(this.Lightness * 100.0, "%");
  }
}
