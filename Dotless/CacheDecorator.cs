// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.CacheDecorator
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Cache;
using Nop.Plugin.InstantSearch.Dotless.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Nop.Plugin.InstantSearch.Dotless
{
  public class CacheDecorator : ILessEngine
  {
    public readonly ILessEngine Underlying;
    public readonly ICache Cache;

    public ILogger Logger { get; set; }

    public CacheDecorator(ILessEngine underlying, ICache cache)
      : this(underlying, cache, (ILogger) NullLogger.Instance)
    {
    }

    public CacheDecorator(ILessEngine underlying, ICache cache, ILogger logger)
    {
      this.Underlying = underlying;
      this.Cache = cache;
      this.Logger = logger;
    }

    public string TransformToCss(string source, string fileName)
    {
      string contentHash = this.ComputeContentHash(source);
      string cacheKey = fileName + contentHash;
      if (!this.Cache.Exists(cacheKey))
      {
        this.Logger.Debug(string.Format("Inserting cache entry for {0}", (object) cacheKey));
        string css = this.Underlying.TransformToCss(source, fileName);
        IEnumerable<string> fileDependancies = ((IEnumerable<string>) new string[1]
        {
          fileName
        }).Concat<string>(this.GetImports());
        this.Cache.Insert(cacheKey, fileDependancies, css);
        return css;
      }
      this.Logger.Debug(string.Format("Retrieving cache entry {0}", (object) cacheKey));
      return this.Cache.Retrieve(cacheKey);
    }

    private string ComputeContentHash(string source) => Convert.ToBase64String(SHA1.Create().ComputeHash(Encoding.Default.GetBytes(source)));

    public IEnumerable<string> GetImports() => this.Underlying.GetImports();

    public void ResetImports() => this.Underlying.ResetImports();

    public bool LastTransformationSuccessful => this.Underlying.LastTransformationSuccessful;

    public string CurrentDirectory
    {
      get => this.Underlying.CurrentDirectory;
      set => this.Underlying.CurrentDirectory = value;
    }
  }
}
