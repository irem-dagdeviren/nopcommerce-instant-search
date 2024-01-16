// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Stylizers.HtmlStylizer
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Parser;

namespace Nop.Plugin.InstantSearch.Dotless.Stylizers
{
  public class HtmlStylizer : IStylizer
  {
    public string Stylize(Zone zone) => string.Format("\r\n<div id=\"less-error-message\">\r\n  <h3>There is an error{0}</h3>\r\n  <p>{1} on line {3}, column {5}</p>\r\n  <div class=\"extract\">\r\n    <pre class=\"before\"><span>{2}</span>{6}</pre>\r\n    <pre class=\"line\"><span>{3}</span>{7}<span class=\"error\">{8}</span>{9}</pre>\r\n    <pre class=\"after\"><span>{4}</span>{10}</pre>\r\n  </div>\r\n</div>\r\n", (object) (string.IsNullOrEmpty(zone.FileName) ? "" : string.Format(" in '{0}'", (object) zone.FileName)), (object) zone.Message, (object) (zone.LineNumber - 1), (object) zone.LineNumber, (object) (zone.LineNumber + 1), (object) zone.Position, (object) zone.Extract.Before, (object) zone.Extract.Line.Substring(0, zone.Position), (object) zone.Extract.Line[zone.Position], (object) zone.Extract.Line.Substring(zone.Position + 1), (object) zone.Extract.After);
  }
}
