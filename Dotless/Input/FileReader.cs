// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Input.FileReader
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using System.IO;

namespace Nop.Plugin.InstantSearch.Dotless.Input
{
  public class FileReader : IFileReader
  {
    public IPathResolver PathResolver { get; set; }

    public FileReader()
      : this((IPathResolver) new RelativePathResolver())
    {
    }

    public FileReader(IPathResolver pathResolver) => this.PathResolver = pathResolver;

    public byte[] GetBinaryFileContents(string fileName)
    {
      fileName = this.PathResolver.GetFullPath(fileName);
      return File.ReadAllBytes(fileName);
    }

    public string GetFileContents(string fileName)
    {
      fileName = this.PathResolver.GetFullPath(fileName);
      return File.ReadAllText(fileName);
    }

    public bool DoesFileExist(string fileName)
    {
      fileName = this.PathResolver.GetFullPath(fileName);
      return File.Exists(fileName);
    }

    public bool UseCacheDependencies => true;
  }
}
