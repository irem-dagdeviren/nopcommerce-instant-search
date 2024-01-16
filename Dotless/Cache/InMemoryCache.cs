// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Cache.InMemoryCache
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using System.Collections.Generic;

namespace Nop.Plugin.InstantSearch.Dotless.Cache
{
  public class InMemoryCache : ICache
  {
    private readonly Dictionary<string, string> _cache;

    public InMemoryCache() => this._cache = new Dictionary<string, string>();

    public void Insert(string fileName, IEnumerable<string> imports, string css) => this._cache[fileName] = css;

    public bool Exists(string filename) => this._cache.ContainsKey(filename);

    public string Retrieve(string filename) => this._cache.ContainsKey(filename) ? this._cache[filename] : "";
  }
}
