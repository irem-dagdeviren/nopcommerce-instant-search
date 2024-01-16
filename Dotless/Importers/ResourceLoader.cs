// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Importers.ResourceLoader
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Nop.Plugin.InstantSearch.Dotless.Importers
{
  internal class ResourceLoader : MarshalByRefObject
  {
    private byte[] _fileContents;
    private string _resourceName;
    private string _resourceContent;

    public static string GetResource(
      string file,
      IFileReader fileReader,
      out string fileDependency)
    {
      fileDependency = (string) null;
      Match match = Importer.EmbeddedResourceRegex.Match(file);
      if (!match.Success)
        return (string) null;
      ResourceLoader loader = new ResourceLoader()
      {
        _resourceName = match.Groups["Resource"].Value
      };
      try
      {
        fileDependency = match.Groups["Assembly"].Value;
        ResourceLoader.LoadFromCurrentAppDomain(loader, fileDependency);
        if (string.IsNullOrEmpty(loader._resourceContent))
          ResourceLoader.LoadFromNewAppDomain(loader, fileReader, fileDependency);
      }
      catch (Exception ex)
      {
        throw new FileNotFoundException("Unable to load resource [" + loader._resourceName + "] in assembly [" + fileDependency + "]");
      }
      finally
      {
        loader._fileContents = (byte[]) null;
      }
      return loader._resourceContent;
    }

    private static void LoadFromCurrentAppDomain(ResourceLoader loader, string assemblyName)
    {
      foreach (Assembly assembly in ((IEnumerable<Assembly>) AppDomain.CurrentDomain.GetAssemblies()).Where<Assembly>((Func<Assembly, bool>) (x => !ResourceLoader.IsDynamicAssembly(x) && x.Location.EndsWith(assemblyName, StringComparison.InvariantCultureIgnoreCase))))
      {
        if (((IEnumerable<string>) assembly.GetManifestResourceNames()).Contains<string>(loader._resourceName))
        {
          using (Stream manifestResourceStream = assembly.GetManifestResourceStream(loader._resourceName))
          {
            using (StreamReader streamReader = new StreamReader(manifestResourceStream))
            {
              loader._resourceContent = streamReader.ReadToEnd();
              if (!string.IsNullOrEmpty(loader._resourceContent))
                break;
            }
          }
        }
      }
    }

    private static bool IsDynamicAssembly(Assembly assembly)
    {
      try
      {
        string location = assembly.Location;
        return false;
      }
      catch (NotSupportedException ex)
      {
        return true;
      }
    }

    private static void LoadFromNewAppDomain(
      ResourceLoader loader,
      IFileReader fileReader,
      string assemblyName)
    {
      loader._fileContents = fileReader.DoesFileExist(assemblyName) ? fileReader.GetBinaryFileContents(assemblyName) : throw new FileNotFoundException("Unable to locate assembly file [" + assemblyName + "]");
      AppDomain domain = AppDomain.CreateDomain("LoaderDomain");
      using (Stream manifestResourceStream = domain.Load(loader._fileContents).GetManifestResourceStream(loader._resourceName))
      {
        using (StreamReader streamReader = new StreamReader(manifestResourceStream))
          loader._resourceContent = streamReader.ReadToEnd();
      }
      AppDomain.Unload(domain);
    }
  }
}
