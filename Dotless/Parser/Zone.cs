// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Zone
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using System;
using System.Linq;

namespace Nop.Plugin.InstantSearch.Dotless.Parser
{
  public class Zone
  {
    public Zone(NodeLocation location)
      : this(location, (string) null, (Zone) null)
    {
    }

    public Zone(NodeLocation location, string error, Zone callZone)
    {
      string source = location.Source;
      if (location.Index > source.Length)
      {
        int length = source.Length;
      }
      int lineNumber;
      int position;
      Zone.GetLineNumber(location, out lineNumber, out position);
      string[] lines = source.Split('\n');
      this.FileName = location.FileName;
      this.Message = error;
      this.CallZone = callZone;
      this.LineNumber = lineNumber + 1;
      this.Position = position;
      this.Extract = new Extract(lines, lineNumber);
    }

    public static int GetLineNumber(NodeLocation location)
    {
      int lineNumber;
      Zone.GetLineNumber(location, out lineNumber, out int _);
      return lineNumber + 1;
    }

    private static void GetLineNumber(NodeLocation location, out int lineNumber, out int position)
    {
      string source1 = location.Source;
      int length = location.Index;
      if (location.Index > source1.Length)
        length = source1.Length;
      string source2 = source1.Substring(0, length);
      int num = source2.LastIndexOf('\n') + 1;
      lineNumber = source2.Count<char>((Func<char, bool>) (c => c == '\n'));
      position = length - num;
    }

    public int LineNumber { get; set; }

    public int Position { get; set; }

    public Extract Extract { get; set; }

    public string Message { get; set; }

    public string FileName { get; set; }

    public Zone CallZone { get; set; }
  }
}
