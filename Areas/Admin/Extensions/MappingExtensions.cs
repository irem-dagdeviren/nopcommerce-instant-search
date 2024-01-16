using Nop.Plugin.InstantSearch.AutoMapper;
using Nop.Plugin.InstantSearch.Areas.Admin.Models;
using Nop.Plugin.InstantSearch.Domain;

namespace Nop.Plugin.InstantSearch.Areas.Admin.Extensions
{
  public static class MappingExtensions
  {
    public static InstantSearchSettingsModel ToModel(this InstantSearchSettings entity) => AutoMapperConfiguration7Spikes.MapTo<InstantSearchSettings, InstantSearchSettingsModel>(entity);

    public static InstantSearchSettings ToEntity(this InstantSearchSettingsModel model) => AutoMapperConfiguration7Spikes.MapTo<InstantSearchSettingsModel, InstantSearchSettings>(model);

    public static InstantSearchSettings ToEntity(
      this InstantSearchSettingsModel model,
      InstantSearchSettings destination)
    {
      return AutoMapperConfiguration7Spikes.MapTo<InstantSearchSettingsModel, InstantSearchSettings>(model, destination);
    }
  }
}
