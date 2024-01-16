// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Stylizers.PlainStylizer
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Parser;

namespace Nop.Plugin.InstantSearch.Dotless.Stylizers
{
  public class PlainStylizer : IStylizer
  {
    public string Stylize(Zone zone)
    {
      string str1 = string.IsNullOrEmpty(zone.FileName) ? "" : string.Format(" in file '{0}'", (object) zone.FileName);
      string str2 = "";
      if (zone.CallZone != null)
      {
        string str3 = "";
        if (zone.CallZone.FileName != zone.FileName && !string.IsNullOrEmpty(zone.CallZone.FileName))
          str3 = string.Format(" in file '{0}'", (object) zone.CallZone.FileName);
        str2 = string.Format("\r\nfrom line {0}{2}:\r\n{0,5:[#]}: {1}", (object) zone.CallZone.LineNumber, (object) zone.CallZone.Extract.Line, (object) str3);
      }
      return string.Format("\r\n{1} on line {4}{0}:\r\n{2,5:[#]}: {3}\r\n{4,5:[#]}: {5}\r\n       {6}^\r\n{7,5:[#]}: {8}{9}", (object) str1, (object) zone.Message, (object) (zone.LineNumber - 1), (object) zone.Extract.Before, (object) zone.LineNumber, (object) zone.Extract.Line, (object) new string('-', zone.Position), (object) (zone.LineNumber + 1), (object) zone.Extract.After, (object) str2);
    }
  }
}
