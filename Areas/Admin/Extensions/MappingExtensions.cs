using Nop.Plugin.InstantSearch.AutoMapper;
using Nop.Plugin.InstantSearch.Areas.Admin.Models;
using Nop.Plugin.InstantSearch.Domain;

namespace Nop.Plugin.InstantSearch.Areas.Admin.Extensions
{
  public static class MappingExtensions
  {
    public static InstantSearchSettingsModel ToModel(this DuzeySearchSettings entity) => AutoMapperConfiguration7Spikes.MapTo<DuzeySearchSettings, InstantSearchSettingsModel>(entity);

    public static DuzeySearchSettings ToEntity(this InstantSearchSettingsModel model) => AutoMapperConfiguration7Spikes.MapTo<InstantSearchSettingsModel, DuzeySearchSettings>(model);

    public static DuzeySearchSettings ToEntity(
      this InstantSearchSettingsModel model,
      DuzeySearchSettings destination)
    {
      return AutoMapperConfiguration7Spikes.MapTo<InstantSearchSettingsModel, DuzeySearchSettings>(model, destination);
    }
  }
}
