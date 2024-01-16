// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Plugins.PluginFinder
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Nop.Plugin.InstantSearch.Dotless.Plugins
{
  public static class PluginFinder
  {
    public static string GetName(this IPlugin plugin) => PluginFinder.GetName(plugin.GetType());

    public static string GetDescription(this IPlugin plugin) => PluginFinder.GetName(plugin.GetType());

    public static string GetDescription(Type pluginType) => ((IEnumerable<object>) pluginType.GetCustomAttributes(typeof (DescriptionAttribute), true)).FirstOrDefault<object>() is DescriptionAttribute descriptionAttribute ? descriptionAttribute.Description : "No Description";

    public static string GetName(Type pluginType) => ((IEnumerable<object>) pluginType.GetCustomAttributes(typeof (DisplayNameAttribute), true)).FirstOrDefault<object>() is DisplayNameAttribute displayNameAttribute ? displayNameAttribute.DisplayName : pluginType.Name;

    public static IEnumerable<IPluginConfigurator> GetConfigurators(bool scanPluginsFolder)
    {
      List<IEnumerable<IPluginConfigurator>> source = new List<IEnumerable<IPluginConfigurator>>();
      source.Add(PluginFinder.GetConfigurators(Assembly.GetAssembly(typeof (PluginFinder))));
      if (scanPluginsFolder)
      {
        string path = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "plugins");
        if (Directory.Exists(path))
        {
          foreach (FileInfo file in new DirectoryInfo(path).GetFiles("*.dll"))
            source.Add(PluginFinder.GetConfigurators(Assembly.LoadFile(file.FullName)));
        }
      }
      return source.Aggregate<IEnumerable<IPluginConfigurator>>((Func<IEnumerable<IPluginConfigurator>, IEnumerable<IPluginConfigurator>, IEnumerable<IPluginConfigurator>>) ((group1, group2) => group1.Union<IPluginConfigurator>(group2)));
    }

    public static IEnumerable<IPluginConfigurator> GetConfigurators(Assembly assembly)
    {
      IEnumerable<Type> source = ((IEnumerable<Type>) assembly.GetTypes()).Where<Type>((Func<Type, bool>) (type => !type.IsAbstract && !type.IsGenericType && !type.IsInterface));
      IEnumerable<IPluginConfigurator> pluginConfigurators = source.Where<Type>((Func<Type, bool>) (type => typeof (IPluginConfigurator).IsAssignableFrom(type))).Select<Type, IPluginConfigurator>((Func<Type, IPluginConfigurator>) (type => (IPluginConfigurator) type.GetConstructor(new Type[0]).Invoke(new object[0])));
      IEnumerable<Type> pluginsConfigurated = pluginConfigurators.Select<IPluginConfigurator, Type>((Func<IPluginConfigurator, Type>) (pluginConfigurator => pluginConfigurator.Configurates));
      Type genericPluginConfiguratorType = typeof (GenericPluginConfigurator<>);
      return source.Where<Type>((Func<Type, bool>) (type => typeof (IPlugin).IsAssignableFrom(type))).Where<Type>((Func<Type, bool>) (type => !pluginsConfigurated.Contains<Type>(type))).Select<Type, IPluginConfigurator>((Func<Type, IPluginConfigurator>) (type => (IPluginConfigurator) Activator.CreateInstance(genericPluginConfiguratorType.MakeGenericType(type)))).Union<IPluginConfigurator>(pluginConfigurators);
    }
  }
}
