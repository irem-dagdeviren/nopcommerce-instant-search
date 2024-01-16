// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Importers.Importer
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Input;
using Nop.Plugin.InstantSearch.Dotless.Parser.Tree;
using Nop.Plugin.InstantSearch.Dotless.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Nop.Plugin.InstantSearch.Dotless.Importers
{
  public class Importer : IImporter
  {
    private static readonly Regex _embeddedResourceRegex = new Regex("^dll://(?<Assembly>.+?)#(?<Resource>.+)$");
    private readonly List<string> _paths = new List<string>();
    protected readonly List<string> _rawImports = new List<string>();
    private readonly List<string> _referenceImports = new List<string>();

    public static Regex EmbeddedResourceRegex => Importer._embeddedResourceRegex;

    public IFileReader FileReader { get; set; }

    public List<string> Imports { get; set; }

    public Func<Nop.Plugin.InstantSearch.Dotless.Parser.Parser> Parser { get; set; }

    public virtual string CurrentDirectory { get; set; }

    public bool IsUrlRewritingDisabled { get; set; }

    public string RootPath { get; set; }

    public bool ImportAllFilesAsLess { get; set; }

    public bool InlineCssFiles { get; set; }

    public Importer()
      : this((IFileReader) new Nop.Plugin.InstantSearch.Dotless.Input.FileReader())
    {
    }

    public Importer(IFileReader fileReader)
      : this(fileReader, false, "", false, false)
    {
    }

    public Importer(
      IFileReader fileReader,
      bool disableUrlReWriting,
      string rootPath,
      bool inlineCssFiles,
      bool importAllFilesAsLess)
    {
      this.FileReader = fileReader;
      this.IsUrlRewritingDisabled = disableUrlReWriting;
      this.RootPath = rootPath;
      this.InlineCssFiles = inlineCssFiles;
      this.ImportAllFilesAsLess = importAllFilesAsLess;
      this.Imports = new List<string>();
      this.CurrentDirectory = "";
    }

    private static bool IsProtocolUrl(string url) => Regex.IsMatch(url, "^([a-zA-Z]{2,}:)");

    private static bool IsNonRelativeUrl(string url) => url.StartsWith("/") || url.StartsWith("~/") || Regex.IsMatch(url, "^[a-zA-Z]:");

    private static bool IsEmbeddedResource(string path) => Importer._embeddedResourceRegex.IsMatch(path);

    public List<string> GetCurrentPathsClone() => new List<string>((IEnumerable<string>) this._paths);

    protected bool CheckIgnoreImport(Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Import import) => this.CheckIgnoreImport(import, import.Path);

    protected bool CheckIgnoreImport(Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Import import, string path)
    {
      if (this.IsOptionSet(import.ImportOptions, ImportOptions.Multiple))
        return false;
      if (!import.IsReference && !this.IsOptionSet(import.ImportOptions, ImportOptions.Reference))
        return this.CheckIgnoreImport(this._rawImports, path);
      return this._rawImports.Contains<string>(path, (IEqualityComparer<string>) StringComparer.InvariantCultureIgnoreCase) || this.CheckIgnoreImport(this._referenceImports, path);
    }

    private bool CheckIgnoreImport(List<string> importList, string path)
    {
      if (importList.Contains<string>(path, (IEqualityComparer<string>) StringComparer.InvariantCultureIgnoreCase))
        return true;
      importList.Add(path);
      return false;
    }

    public virtual ImportAction Import(Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Import import)
    {
      if (Importer.IsProtocolUrl(import.Path) && !Importer.IsEmbeddedResource(import.Path))
      {
        if (import.Path.EndsWith(".less"))
          throw new FileNotFoundException(string.Format(".less cannot import non local less files [{0}].", (object) import.Path), import.Path);
        return this.CheckIgnoreImport(import) ? ImportAction.ImportNothing : ImportAction.LeaveImport;
      }
      string str = import.Path;
      if (!Importer.IsNonRelativeUrl(str))
        str = this.GetAdjustedFilePath(import.Path, (IEnumerable<string>) this._paths);
      if (this.CheckIgnoreImport(import, str))
        return ImportAction.ImportNothing;
      if ((this.ImportAllFilesAsLess ? 1 : (this.IsOptionSet(import.ImportOptions, ImportOptions.Less) ? 1 : 0)) == 0 && import.Path.EndsWith(".css") && !import.Path.EndsWith(".less.css"))
        return (this.InlineCssFiles || this.IsOptionSet(import.ImportOptions, ImportOptions.Inline)) && (Importer.IsEmbeddedResource(import.Path) && this.ImportEmbeddedCssContents(str, import) || this.ImportCssFileContents(str, import)) ? ImportAction.ImportCss : ImportAction.LeaveImport;
      if (this.Parser == null)
        throw new InvalidOperationException("Parser cannot be null.");
      if (this.ImportLessFile(str, import))
        return ImportAction.ImportLess;
      if (this.IsOptionSet(import.ImportOptions, ImportOptions.Optional))
        return ImportAction.ImportNothing;
      if (this.IsOptionSet(import.ImportOptions, ImportOptions.Css) || !import.Path.EndsWith(".less", StringComparison.InvariantCultureIgnoreCase))
        return ImportAction.LeaveImport;
      throw new FileNotFoundException(string.Format("You are importing a file ending in .less that cannot be found [{0}].", (object) str), str);
    }

    public IDisposable BeginScope(Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Import parentScope) => (IDisposable) new Importer.ImportScope(this, Path.GetDirectoryName(parentScope.Path));

    protected string GetAdjustedFilePath(string path, IEnumerable<string> pathList) => pathList.Concat<string>((IEnumerable<string>) new string[1]
    {
      path
    }).AggregatePaths(this.CurrentDirectory);

    protected bool ImportLessFile(string lessPath, Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Import import)
    {
      string fileDependency = (string) null;
      string input;
      if (Importer.IsEmbeddedResource(lessPath))
      {
        input = ResourceLoader.GetResource(lessPath, this.FileReader, out fileDependency);
        if (input == null)
          return false;
      }
      else
      {
        string fileName = lessPath;
        if (Path.IsPathRooted(lessPath))
          fileName = lessPath;
        else if (!string.IsNullOrEmpty(this.CurrentDirectory))
          fileName = this.CurrentDirectory.Replace("\\", "/").TrimEnd('/') + "/" + lessPath;
        bool flag = this.FileReader.DoesFileExist(fileName);
        if (!flag && !fileName.EndsWith(".less"))
        {
          fileName += ".less";
          flag = this.FileReader.DoesFileExist(fileName);
        }
        if (!flag)
          return false;
        input = this.FileReader.GetFileContents(fileName);
        fileDependency = fileName;
      }
      this._paths.Add(Path.GetDirectoryName(import.Path));
      try
      {
        if (!string.IsNullOrEmpty(fileDependency))
          this.Imports.Add(fileDependency);
        import.InnerRoot = this.Parser().Parse(input, lessPath);
      }
      catch
      {
        this.Imports.Remove(fileDependency);
        throw;
      }
      finally
      {
        this._paths.RemoveAt(this._paths.Count - 1);
      }
      return true;
    }

    private bool ImportEmbeddedCssContents(string file, Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Import import)
    {
      string resource = ResourceLoader.GetResource(file, this.FileReader, out file);
      if (resource == null)
        return false;
      import.InnerContent = resource;
      return true;
    }

    protected bool ImportCssFileContents(string file, Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Import import)
    {
      if (!this.FileReader.DoesFileExist(file))
        return false;
      import.InnerContent = this.FileReader.GetFileContents(file);
      this.Imports.Add(file);
      return true;
    }

    public string AlterUrl(string url, List<string> pathList)
    {
      if (Importer.IsProtocolUrl(url) || Importer.IsNonRelativeUrl(url))
        return url;
      if (pathList.Any<string>() && !this.IsUrlRewritingDisabled)
        url = this.GetAdjustedFilePath(url, (IEnumerable<string>) pathList);
      return this.RootPath + url;
    }

    public void ResetImports()
    {
      this.Imports.Clear();
      this._rawImports.Clear();
    }

    public IEnumerable<string> GetImports() => this.Imports.Distinct<string>();

    private bool IsOptionSet(ImportOptions options, ImportOptions test) => (options & test) == test;

    private class ImportScope : IDisposable
    {
      private readonly Importer importer;

      public ImportScope(Importer importer, string path)
      {
        this.importer = importer;
        this.importer._paths.Add(path);
      }

      public void Dispose() => this.importer._paths.RemoveAt(this.importer._paths.Count - 1);
    }
  }
}
