using Nop.Plugin.InstantSearch.Dotless.Exceptions;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes;
using Nop.Plugin.InstantSearch.Dotless.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Nop.Plugin.InstantSearch.Dotless.Parser.Tree
{
  public class Color : Node, IOperable, IComparable
  {
    private static readonly Dictionary<string, int> Html4Colors = new Dictionary<string, int>()
    {
      {
        "aliceblue",
        15792383
      },
      {
        "antiquewhite",
        16444375
      },
      {
        "aqua",
        (int) ushort.MaxValue
      },
      {
        "aquamarine",
        8388564
      },
      {
        "azure",
        15794175
      },
      {
        "beige",
        16119260
      },
      {
        "bisque",
        16770244
      },
      {
        "black",
        0
      },
      {
        "blanchedalmond",
        16772045
      },
      {
        "blue",
        (int) byte.MaxValue
      },
      {
        "blueviolet",
        9055202
      },
      {
        "brown",
        10824234
      },
      {
        "burlywood",
        14596231
      },
      {
        "cadetblue",
        6266528
      },
      {
        "chartreuse",
        8388352
      },
      {
        "chocolate",
        13789470
      },
      {
        "coral",
        16744272
      },
      {
        "cornflowerblue",
        6591981
      },
      {
        "cornsilk",
        16775388
      },
      {
        "crimson",
        14423100
      },
      {
        "cyan",
        (int) ushort.MaxValue
      },
      {
        "darkblue",
        139
      },
      {
        "darkcyan",
        35723
      },
      {
        "darkgoldenrod",
        12092939
      },
      {
        "darkgray",
        11119017
      },
      {
        "darkgrey",
        11119017
      },
      {
        "darkgreen",
        25600
      },
      {
        "darkkhaki",
        12433259
      },
      {
        "darkmagenta",
        9109643
      },
      {
        "darkolivegreen",
        5597999
      },
      {
        "darkorange",
        16747520
      },
      {
        "darkorchid",
        10040012
      },
      {
        "darkred",
        9109504
      },
      {
        "darksalmon",
        15308410
      },
      {
        "darkseagreen",
        9419919
      },
      {
        "darkslateblue",
        4734347
      },
      {
        "darkslategray",
        3100495
      },
      {
        "darkslategrey",
        3100495
      },
      {
        "darkturquoise",
        52945
      },
      {
        "darkviolet",
        9699539
      },
      {
        "deeppink",
        16716947
      },
      {
        "deepskyblue",
        49151
      },
      {
        "dimgray",
        6908265
      },
      {
        "dimgrey",
        6908265
      },
      {
        "dodgerblue",
        2003199
      },
      {
        "firebrick",
        11674146
      },
      {
        "floralwhite",
        16775920
      },
      {
        "forestgreen",
        2263842
      },
      {
        "fuchsia",
        16711935
      },
      {
        "gainsboro",
        14474460
      },
      {
        "ghostwhite",
        16316671
      },
      {
        "gold",
        16766720
      },
      {
        "goldenrod",
        14329120
      },
      {
        "gray",
        8421504
      },
      {
        "grey",
        8421504
      },
      {
        "green",
        32768
      },
      {
        "greenyellow",
        11403055
      },
      {
        "honeydew",
        15794160
      },
      {
        "hotpink",
        16738740
      },
      {
        "indianred",
        13458524
      },
      {
        "indigo",
        4915330
      },
      {
        "ivory",
        16777200
      },
      {
        "khaki",
        15787660
      },
      {
        "lavender",
        15132410
      },
      {
        "lavenderblush",
        16773365
      },
      {
        "lawngreen",
        8190976
      },
      {
        "lemonchiffon",
        16775885
      },
      {
        "lightblue",
        11393254
      },
      {
        "lightcoral",
        15761536
      },
      {
        "lightcyan",
        14745599
      },
      {
        "lightgoldenrodyellow",
        16448210
      },
      {
        "lightgray",
        13882323
      },
      {
        "lightgrey",
        13882323
      },
      {
        "lightgreen",
        9498256
      },
      {
        "lightpink",
        16758465
      },
      {
        "lightsalmon",
        16752762
      },
      {
        "lightseagreen",
        2142890
      },
      {
        "lightskyblue",
        8900346
      },
      {
        "lightslategray",
        7833753
      },
      {
        "lightslategrey",
        7833753
      },
      {
        "lightsteelblue",
        11584734
      },
      {
        "lightyellow",
        16777184
      },
      {
        "lime",
        65280
      },
      {
        "limegreen",
        3329330
      },
      {
        "linen",
        16445670
      },
      {
        "magenta",
        16711935
      },
      {
        "maroon",
        8388608
      },
      {
        "mediumaquamarine",
        6737322
      },
      {
        "mediumblue",
        205
      },
      {
        "mediumorchid",
        12211667
      },
      {
        "mediumpurple",
        9662680
      },
      {
        "mediumseagreen",
        3978097
      },
      {
        "mediumslateblue",
        8087790
      },
      {
        "mediumspringgreen",
        64154
      },
      {
        "mediumturquoise",
        4772300
      },
      {
        "mediumvioletred",
        13047173
      },
      {
        "midnightblue",
        1644912
      },
      {
        "mintcream",
        16121850
      },
      {
        "mistyrose",
        16770273
      },
      {
        "moccasin",
        16770229
      },
      {
        "navajowhite",
        16768685
      },
      {
        "navy",
        128
      },
      {
        "oldlace",
        16643558
      },
      {
        "olive",
        8421376
      },
      {
        "olivedrab",
        7048739
      },
      {
        "orange",
        16753920
      },
      {
        "orangered",
        16729344
      },
      {
        "orchid",
        14315734
      },
      {
        "palegoldenrod",
        15657130
      },
      {
        "palegreen",
        10025880
      },
      {
        "paleturquoise",
        11529966
      },
      {
        "palevioletred",
        14184595
      },
      {
        "papayawhip",
        16773077
      },
      {
        "peachpuff",
        16767673
      },
      {
        "peru",
        13468991
      },
      {
        "pink",
        16761035
      },
      {
        "plum",
        14524637
      },
      {
        "powderblue",
        11591910
      },
      {
        "purple",
        8388736
      },
      {
        "red",
        16711680
      },
      {
        "rosybrown",
        12357519
      },
      {
        "royalblue",
        4286945
      },
      {
        "saddlebrown",
        9127187
      },
      {
        "salmon",
        16416882
      },
      {
        "sandybrown",
        16032864
      },
      {
        "seagreen",
        3050327
      },
      {
        "seashell",
        16774638
      },
      {
        "sienna",
        10506797
      },
      {
        "silver",
        12632256
      },
      {
        "skyblue",
        8900331
      },
      {
        "slateblue",
        6970061
      },
      {
        "slategray",
        7372944
      },
      {
        "slategrey",
        7372944
      },
      {
        "snow",
        16775930
      },
      {
        "springgreen",
        65407
      },
      {
        "steelblue",
        4620980
      },
      {
        "tan",
        13808780
      },
      {
        "teal",
        32896
      },
      {
        "thistle",
        14204888
      },
      {
        "tomato",
        16737095
      },
      {
        "turquoise",
        4251856
      },
      {
        "violet",
        15631086
      },
      {
        "wheat",
        16113331
      },
      {
        "white",
        16777215
      },
      {
        "whitesmoke",
        16119285
      },
      {
        "yellow",
        16776960
      },
      {
        "yellowgreen",
        10145074
      }
    };
    private static readonly Dictionary<int, string> Html4ColorsReverse = Color.Html4Colors.GroupBy<KeyValuePair<string, int>, int>((Func<KeyValuePair<string, int>, int>) (kvp => kvp.Value)).ToDictionary<IGrouping<int, KeyValuePair<string, int>>, int, string>((Func<IGrouping<int, KeyValuePair<string, int>>, int>) (g => g.Key), (Func<IGrouping<int, KeyValuePair<string, int>>, string>) (g => g.First<KeyValuePair<string, int>>().Key));
    private readonly string _text;
    public readonly double[] RGB = new double[3];
    public readonly double Alpha;

    public static Color From(string keywordOrHex) => Color.GetColorFromKeyword(keywordOrHex) ?? Color.FromHex(keywordOrHex);

    public static Color GetColorFromKeyword(string keyword)
    {
      if (keyword == "transparent")
        return new Color(0.0, 0.0, 0.0, 0.0, keyword);
      int num;
      return Color.Html4Colors.TryGetValue(keyword, out num) ? new Color((double) (num >> 16 & (int) byte.MaxValue), (double) (num >> 8 & (int) byte.MaxValue), (double) (num & (int) byte.MaxValue), text: keyword) : (Color) null;
    }

    public static string GetKeyword(int[] rgb)
    {
      int key = (rgb[0] << 16) + (rgb[1] << 8) + rgb[2];
      string str;
      return Color.Html4ColorsReverse.TryGetValue(key, out str) ? str : (string) null;
    }

    public static Color FromHex(string hex)
    {
      hex = hex.TrimStart('#');
      double alpha = 1.0;
      string text = "#" + hex;
      double[] rgb;
      if (hex.Length == 8)
      {
        rgb = Color.ParseRgb(hex.Substring(2));
        alpha = Color.Parse(hex.Substring(0, 2)) / (double) byte.MaxValue;
      }
      else
        rgb = hex.Length != 6 ? ((IEnumerable<char>) hex.ToCharArray()).Select<char, double>((Func<char, double>) (c => Color.Parse(c.ToString() + c.ToString()))).ToArray<double>() : Color.ParseRgb(hex);
      return new Color(rgb, alpha, text);
    }

    private static double[] ParseRgb(string hex) => Enumerable.Range(0, 3).Select<int, string>((Func<int, string>) (i => hex.Substring(i * 2, 2))).Select<string, double>(new Func<string, double>(Color.Parse)).ToArray<double>();

    private static double Parse(string hex) => (double) int.Parse(hex, NumberStyles.HexNumber);

    public Color(int color)
    {
      this.RGB = new double[3];
      this.B = (double) (color & (int) byte.MaxValue);
      color >>= 8;
      this.G = (double) (color & (int) byte.MaxValue);
      color >>= 8;
      this.R = (double) (color & (int) byte.MaxValue);
      this.Alpha = 1.0;
    }

    public Color(string hex)
    {
      hex = hex.TrimStart('#');
      double num = 1.0;
      string str = "#" + hex;
      double[] numArray;
      if (hex.Length == 8)
      {
        numArray = Color.ParseRgb(hex.Substring(2));
        num = Color.Parse(hex.Substring(0, 2)) / (double) byte.MaxValue;
      }
      else
        numArray = hex.Length != 6 ? ((IEnumerable<char>) hex.ToCharArray()).Select<char, double>((Func<char, double>) (c => Color.Parse(c.ToString() + c.ToString()))).ToArray<double>() : Color.ParseRgb(hex);
      this.R = numArray[0];
      this.G = numArray[1];
      this.B = numArray[2];
      this.Alpha = num;
      this._text = str;
    }

    public Color(IEnumerable<Number> rgb, Number alpha)
    {
      this.RGB = rgb.Select<Number, double>((Func<Number, double>) (d => d.Normalize())).ToArray<double>();
      this.Alpha = alpha.Normalize();
    }

    public Color(double r, double g, double b)
      : this(r, g, b, 1.0)
    {
    }

    public Color(double r, double g, double b, double alpha)
      : this(new double[3]{ r, g, b }, alpha)
    {
    }

    public Color(double[] rgb)
      : this(rgb, 1.0)
    {
    }

    public Color(double[] rgb, double alpha)
      : this(rgb, alpha, (string) null)
    {
    }

    public Color(double[] rgb, double alpha, string text)
    {
      this.RGB = ((IEnumerable<double>) rgb).Select<double, double>((Func<double, double>) (c => NumberExtensions.Normalize(c, (double) byte.MaxValue))).ToArray<double>();
      this.Alpha = NumberExtensions.Normalize(alpha, 1.0);
      this._text = text;
    }

    public Color(double red, double green, double blue, double alpha = 1.0, string text = null)
      : this(new double[3]{ red, green, blue }, alpha, text)
    {
    }

    public double R
    {
      get => this.RGB[0];
      set => this.RGB[0] = value;
    }

    public double G
    {
      get => this.RGB[1];
      set => this.RGB[1] = value;
    }

    public double B
    {
      get => this.RGB[2];
      set => this.RGB[2] = value;
    }

    private double TransformLinearToSrbg(double linearChannel) => linearChannel > 0.03928 ? Math.Pow((linearChannel + 0.055) / 1.055, 2.4) : linearChannel / 12.92;

    public double Luma
    {
      get
      {
        double linearChannel1 = this.R / (double) byte.MaxValue;
        double linearChannel2 = this.G / (double) byte.MaxValue;
        double linearChannel3 = this.B / (double) byte.MaxValue;
        return 0.2126 * this.TransformLinearToSrbg(linearChannel1) + 447.0 / 625.0 * this.TransformLinearToSrbg(linearChannel2) + 0.0722 * this.TransformLinearToSrbg(linearChannel3);
      }
    }

    protected override Node CloneCore() => (Node) new Color(((IEnumerable<double>) this.RGB).ToArray<double>(), this.Alpha);

    public override void AppendCSS(Env env)
    {
      if (this._text != null)
      {
        env.Output.Append(this._text);
      }
      else
      {
        List<int> rgb = this.ConvertToInt((IEnumerable<double>) this.RGB);
        if (this.Alpha < 1.0)
        {
          env.Output.AppendFormat("rgba({0}, {1}, {2}, {3})", (object) rgb[0], (object) rgb[1], (object) rgb[2], (object) this.Alpha);
        }
        else
        {
          string hexString = this.GetHexString((IEnumerable<int>) rgb);
          env.Output.Append(hexString);
        }
      }
    }

    private List<int> ConvertToInt(IEnumerable<double> rgb) => rgb.Select<double, int>((Func<double, int>) (d => (int) Math.Round(d, MidpointRounding.AwayFromZero))).ToList<int>();

    private string GetHexString(IEnumerable<int> rgb) => "#" + rgb.Select<int, string>((Func<int, string>) (i => i.ToString("x2"))).JoinStrings("");

    public Node Operate(Operation op, Node other)
    {
      Color color = other as Color;
      IOperable operable = other as IOperable;
      if (color == null && operable == null)
        throw new ParsingException(string.Format("Unable to convert right hand side of {0} to a color", (object) op.Operator), op.Location);
      color = color ?? operable.ToColor();
      return new Color(Enumerable.Range(0, 3).Select<int, double>((Func<int, double>) (i => Operation.Operate(op.Operator, this.RGB[i], color.RGB[i]))).ToArray<double>(), 1.0).ReducedFrom<Node>((Node) this, other);
    }

    public Color ToColor() => this;

    public string ToArgb() => this.GetHexString((IEnumerable<int>) this.ConvertToInt(((IEnumerable<double>) new double[1]
    {
      this.Alpha * (double) byte.MaxValue
    }).Concat<double>((IEnumerable<double>) this.RGB)));

    public int CompareTo(object obj)
    {
      if (!(obj is Color color))
        return -1;
      if (color.R == this.R && color.G == this.G && color.B == this.B && color.Alpha == this.Alpha)
        return 0;
      return (768.0 - (color.R + color.G + color.B)) * color.Alpha >= (768.0 - (this.R + this.G + this.B)) * this.Alpha ? -1 : 1;
    }

    public static explicit operator System.Drawing.Color(Color color)
    {
      if (color == null)
        throw new ArgumentNullException(nameof (color));
      return System.Drawing.Color.FromArgb((int) Math.Round(color.Alpha * (double) byte.MaxValue), (int) color.R, (int) color.G, (int) color.B);
    }
  }
}
