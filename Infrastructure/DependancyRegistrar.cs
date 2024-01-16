using Nop.Plugin.InstantSearch.DependancyRegistrar;
using Nop.Plugin.InstantSearch.Areas.Admin.Models;
using Nop.Plugin.InstantSearch.Domain;

namespace Nop.Plugin.InstantSearch.Infrastructure
{
  public class DependancyRegistrar : BaseDependancyRegistrar7Spikes
  {
    protected override void CreateModelMappings() => this.CreateMvcModelMap<InstantSearchSettingsModel, InstantSearchSettings>();
  }
}
