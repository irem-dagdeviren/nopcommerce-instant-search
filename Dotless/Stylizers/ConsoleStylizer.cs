// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Stylizers.ConsoleStylizer
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Parser;
using System.Collections.Generic;
using System.Text;

namespace Nop.Plugin.InstantSearch.Dotless.Stylizers
{
  public class ConsoleStylizer : IStylizer
  {
    private Dictionary<string, int[]> styles;

    public ConsoleStylizer() => this.styles = new Dictionary<string, int[]>()
    {
      {
        "bold",
        new int[2]{ 1, 22 }
      },
      {
        "inverse",
        new int[2]{ 7, 27 }
      },
      {
        "underline",
        new int[2]{ 4, 24 }
      },
      {
        "yellow",
        new int[2]{ 33, 39 }
      },
      {
        "green",
        new int[2]{ 32, 39 }
      },
      {
        "red",
        new int[2]{ 31, 39 }
      },
      {
        "grey",
        new int[2]{ 90, 39 }
      },
      {
        "reset",
        new int[2]
      }
    };

    private string Stylize(string str, string style) => "\u001B[" + (object) this.styles[style][0] + "m" + str + "\u001B[" + (object) this.styles[style][1] + "m";

    public string Stylize(Zone zone)
    {
      Extract extract = zone.Extract;
      int position = zone.Position;
      string str1 = extract.Line.Substring(0, position);
      string str2 = extract.Line.Substring(position + 1);
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(this.Stylize(extract.Before, "grey"));
      stringBuilder.Append(this.Stylize(str1, "green"));
      stringBuilder.Append(this.Stylize(this.Stylize(extract.Line[position].ToString(), "inverse") + str2, "yellow"));
      stringBuilder.Append(this.Stylize(extract.After, "grey"));
      stringBuilder.Append(this.Stylize("", "reset"));
      return stringBuilder.ToString();
    }
  }
}
