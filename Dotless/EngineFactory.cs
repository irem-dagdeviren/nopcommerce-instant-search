// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.EngineFactory
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Nop.Plugin.InstantSearch.Dotless
{
  public class EngineFactory
  {
    public DotlessConfiguration Configuration { get; set; }

    public EngineFactory(DotlessConfiguration configuration) => this.Configuration = configuration;

    public EngineFactory()
      : this(DotlessConfiguration.GetDefault())
    {
    }

    public ILessEngine GetEngine() => this.GetEngine(new ContainerFactory());

    public ILessEngine GetEngine(ContainerFactory containerFactory) => ServiceProviderServiceExtensions.GetRequiredService<ILessEngine>(containerFactory.GetContainer(this.Configuration));
  }
}
