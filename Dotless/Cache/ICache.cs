// Decompiled with JetBrains decompiler
// Type:
// .Cache.ICache
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using System.Collections.Generic;

namespace Nop.Plugin.InstantSearch.Dotless.Cache
{
  public interface ICache
  {
    void Insert(string cacheKey, IEnumerable<string> fileDependancies, string css);

    bool Exists(string cacheKey);

    string Retrieve(string cacheKey);
  }
}
