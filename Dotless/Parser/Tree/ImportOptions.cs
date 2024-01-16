// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Tree.ImportOptions
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using System;

namespace Nop.Plugin.InstantSearch.Dotless.Parser.Tree
{
  [Flags]
  public enum ImportOptions
  {
    Once = 1,
    Multiple = 2,
    Optional = 4,
    Css = 8,
    Less = 16, // 0x00000010
    Inline = 32, // 0x00000020
    Reference = 64, // 0x00000040
  }
}
