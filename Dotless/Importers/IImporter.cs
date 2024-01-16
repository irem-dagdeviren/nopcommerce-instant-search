// Decompiled with JetBrains decompiler
// Type:
//
// .Importers.IImporter
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using System;
using System.Collections.Generic;

namespace Nop.Plugin.InstantSearch.Dotless.Importers
{
  public interface IImporter
  {
    List<string> GetCurrentPathsClone();

    ImportAction Import(Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Import import);

    Func<Nop.Plugin.InstantSearch.Dotless.Parser.Parser> Parser { get; set; }

    string AlterUrl(string url, List<string> pathList);

    string CurrentDirectory { get; set; }

    IDisposable BeginScope(Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Import parent);

    void ResetImports();

    IEnumerable<string> GetImports();
  }
}
