// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Extract
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

namespace Nop.Plugin.InstantSearch.Dotless.Parser
{
  public class Extract
  {
    public Extract(string[] lines, int line)
    {
      this.Before = line > 0 ? lines[line - 1] : "/beginning of file";
      this.Line = lines[line];
      this.After = line + 1 < lines.Length ? lines[line + 1] : "/end of file";
    }

    public string After { get; set; }

    public string Before { get; set; }

    public string Line { get; set; }
  }
}
