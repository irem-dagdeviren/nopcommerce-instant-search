using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;
using Nop.Plugin.InstantSearch.AutoMapper;

namespace Nop.Plugin.InstantSearch.DependancyRegistrar
{
  public abstract class BaseDependancyRegistrar7Spikes : INopStartup
  {
    protected virtual void CreateModelMappings()
    {
    }

    protected void CreateMvcModelMap<TModel, TEntity>()
    {
      AutoMapperConfiguration7Spikes.MapperConfigurationExpression.CreateMap<TModel, TEntity>();
      AutoMapperConfiguration7Spikes.MapperConfigurationExpression.CreateMap<TEntity, TModel>();
    }

    protected virtual void RegisterPluginServices(IServiceCollection services)
    {
    }

    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
      this.RegisterPluginServices(services);
      this.CreateModelMappings();
    }

    public void Configure(IApplicationBuilder application)
    {
    }

    public virtual int Order => 7000;
  }
}
