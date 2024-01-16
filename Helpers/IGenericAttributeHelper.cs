using Nop.Plugin.InstantSearch.Domain;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Nop.Plugin.InstantSearch.Helpers
{
  public interface IGenericAttributeHelper
  {
    Task MoveCustomHeadStylesFromSettingsToGenericAttributeAsync<T>(
      string settingKey,
      string pluginSystemName,
      T themeSettings)
      where T : Base7SpikesThemeSettings, new();

    Task MovePresetFromSettingsToGenericAttributeAsync(string settingKey, string pluginSystemName);

    Task AddThemeAttributeAsync<T>(
      NopTemplatesThemeConfiguration themeConfiguration,
      Expression<Func<T>> property,
      string pluginSystemName,
      int storeId);

    Task<string> GetThemeAttributeAsync<T>(
      NopTemplatesThemeConfiguration themeConfiguration,
      Expression<Func<T>> property,
      string pluginSystemName,
      int storeId = 0,
      bool loadSharedValueIfNotFound = true);

    Task RemoveThemeAttributeAsync<T>(
      Expression<Func<T>> property,
      string pluginSystemName,
      int storeId = 0);

    Task SaveCustomMapStylesAsync(string customMapStyles, string pluginSystemName, int storeId);

    Task<string> GetCustomMapStylesAsync(
      string pluginSystemName,
      int storeId,
      bool loadSharedValue = true);
  }
}
