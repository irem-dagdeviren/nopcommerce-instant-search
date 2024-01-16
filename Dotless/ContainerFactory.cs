// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.ContainerFactory
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Cache;
using Nop.Plugin.InstantSearch.Dotless.configuration;
using Nop.Plugin.InstantSearch.Dotless.Importers;
using Nop.Plugin.InstantSearch.Dotless.Input;
using Nop.Plugin.InstantSearch.Dotless.Loggers;
using Nop.Plugin.InstantSearch.Dotless.Parameters;
using Nop.Plugin.InstantSearch.Dotless.Plugins;
using Nop.Plugin.InstantSearch.Dotless.Stylizers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Nop.Plugin.InstantSearch.Dotless
{
  public class ContainerFactory
  {
    protected IServiceCollection Container { get; set; }

    public IServiceProvider GetContainer(DotlessConfiguration configuration) => (IServiceProvider) ServiceCollectionContainerBuilderExtensions.BuildServiceProvider((IServiceCollection) this.GetServices(configuration));

    public ServiceCollection GetServices(DotlessConfiguration configuration)
    {
      ServiceCollection services = new ServiceCollection();
      this.RegisterServices((IServiceCollection) services, configuration);
      return services;
    }

    protected virtual void RegisterServices(
      IServiceCollection services,
      DotlessConfiguration configuration)
    {
      if (!configuration.Web)
        this.RegisterLocalServices(services);
      this.RegisterCoreServices(services, configuration);
      this.OverrideServices(services, configuration);
    }

    protected virtual void OverrideServices(
      IServiceCollection services,
      DotlessConfiguration configuration)
    {
      if (!(configuration.Logger != (Type) null))
        return;
      ServiceCollectionServiceExtensions.AddSingleton(services, typeof (ILogger), configuration.Logger);
    }

    protected virtual void RegisterLocalServices(IServiceCollection services)
    {
      services.AddSingleton<ICache, InMemoryCache>();
      services.AddSingleton<IParameterSource, ConsoleArgumentParameterSource>();
      services.AddSingleton<ILogger, ConsoleLogger>();
      services.AddSingleton<IPathResolver, RelativePathResolver>();
    }

    protected virtual void RegisterCoreServices(
      IServiceCollection services,
      DotlessConfiguration configuration)
    {
      services.AddSingleton<DotlessConfiguration>(configuration);
      services.AddSingleton<IStylizer, PlainStylizer>();
      services.AddTransient<IImporter, Importer>();
      services.AddTransient<Nop.Plugin.InstantSearch.Dotless.Parser.Parser>();
      services.AddTransient<ILessEngine, LessEngine>();
      if (configuration.CacheEnabled)
        services.Decorate<ILessEngine, CacheDecorator>();
      if (!configuration.DisableParameters)
        services.Decorate<ILessEngine, ParameterDecorator>();
      services.AddSingleton<IEnumerable<IPluginConfigurator>>((IEnumerable<IPluginConfigurator>) configuration.Plugins);
      ServiceCollectionServiceExtensions.AddSingleton(services, typeof (IFileReader), configuration.LessSource);
    }
  }
}
