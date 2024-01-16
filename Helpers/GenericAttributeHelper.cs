using Nop.Core;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Stores;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Stores;
using Nop.Plugin.InstantSearch.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;


#nullable enable
namespace Nop.Plugin.InstantSearch.Helpers
{
    public class GenericAttributeHelper : IGenericAttributeHelper
    {
        private const
#nullable disable
        string KeyPattern = "{0}-{1}";
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ISettingService _settingsService;
        private readonly IStoreService _storeService;
        private readonly IPresetsHelper _presetsHelper;

        public GenericAttributeHelper(
          IGenericAttributeService genericAttributeService,
          ISettingService settingsService,
          IStoreService storeService,
          IPresetsHelper presetsHelper)
        {
            this._genericAttributeService = genericAttributeService;
            this._settingsService = settingsService;
            this._storeService = storeService;
            this._presetsHelper = presetsHelper;
        }

        public async Task AddThemeAttributeAsync<T>(
          NopTemplatesThemeConfiguration themeConfiguration,
          Expression<Func<T>> property,
          string pluginSystemName,
          int storeId)
        {
            MemberExpression body = (MemberExpression)property.Body;
            string str = string.Format("{0}-{1}", (object)body.Member.Name, (object)pluginSystemName);
            object obj = ((object)themeConfiguration).GetType().GetProperty(body.Member.Name).GetValue((object)themeConfiguration, (object[])null);
            if (obj == null)
                return;
            await this._genericAttributeService.SaveAttributeAsync<object>((BaseEntity)themeConfiguration, str, obj, storeId);
        }

        public Task<string> GetCustomMapStylesAsync(string pluginSystemName, int storeId, bool loadSharedValue = true)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetThemeAttributeAsync<T>(
      NopTemplatesThemeConfiguration themeConfiguration,
      Expression<Func<T>> property,
      string pluginSystemName,
      int storeId = 0,
      bool loadSharedValueIfNotFound = true)
        {
            string key = string.Format("{0}-{1}", (object)((MemberExpression)property.Body).Member.Name, (object)pluginSystemName);
            string attributeAsync = await this._genericAttributeService.GetAttributeAsync<string>((BaseEntity)themeConfiguration, key, storeId, (string)null);
            if (((!string.IsNullOrWhiteSpace(attributeAsync) ? 0 : (storeId > 0 ? 1 : 0)) & (loadSharedValueIfNotFound ? 1 : 0)) != 0)
                attributeAsync = await this._genericAttributeService.GetAttributeAsync<string>((BaseEntity)themeConfiguration, key, 0, (string)null);
            string themeAttributeAsync = attributeAsync;
            key = (string)null;
            return themeAttributeAsync;
        }

        public async Task RemoveThemeAttributeAsync<T>(
          Expression<Func<T>> property,
          string pluginSystemName,
          int storeId = 0)
        {
            IList<GenericAttribute> attributesForEntityAsync = await this._genericAttributeService.GetAttributesForEntityAsync(0, typeof(NopTemplatesThemeConfiguration).Name);
            string key = string.Format("{0}-{1}", (object)((MemberExpression)property.Body).Member.Name, (object)pluginSystemName);
            Func<GenericAttribute, bool> predicate = (Func<GenericAttribute, bool>)(x => x.StoreId == storeId && x.Key == key);
            GenericAttribute genericAttribute = attributesForEntityAsync.FirstOrDefault<GenericAttribute>(predicate);
            if (genericAttribute == null)
                ;
            else
                await this._genericAttributeService.DeleteAttributeAsync(genericAttribute);
        }

        public Task SaveCustomMapStylesAsync(string customMapStyles, string pluginSystemName, int storeId)
        {
            throw new NotImplementedException();
        }

        async Task IGenericAttributeHelper.MoveCustomHeadStylesFromSettingsToGenericAttributeAsync<T>(
      string settingKey,
      string pluginSystemName,
      T themeSettings)
        {
            IList<Store> stores = await this._storeService.GetAllStoresAsync();
            bool settingByKeyAsync = await this._settingsService.GetSettingByKeyAsync<bool>("SevenSpikesCommonSettings.LoadStoreSettingsOnLoad", true, 0, false);
            if (stores.Count < 2 || !settingByKeyAsync)
            {
                await this.MoveCustomHeadStylesFromSettingsToGenericAttributeForStoreAsync<T>(settingKey, pluginSystemName, themeSettings, 0);
                stores = (IList<Store>)null;
            }
            else
            {
                foreach (BaseEntity baseEntity in (IEnumerable<Store>)stores)
                {
                    int id = baseEntity.Id;
                    await this.MoveCustomHeadStylesFromSettingsToGenericAttributeForStoreAsync<T>(settingKey, pluginSystemName, themeSettings, id);
                }
                stores = (IList<Store>)null;
            }
        }

        private async Task MoveCustomHeadStylesFromSettingsToGenericAttributeForStoreAsync<T>(
          string settingKey,
          string pluginSystemName,
          T themeSettings,
          int storeId)
          where T : Base7SpikesThemeSettings, new()
        {
            string settingByKeyAsync = await this._settingsService.GetSettingByKeyAsync<string>(settingKey, (string)null, storeId, false);
            if (string.IsNullOrEmpty(settingByKeyAsync))
                return;
            NopTemplatesThemeConfiguration themeConfiguration1 = new NopTemplatesThemeConfiguration();
            themeConfiguration1.Id = 0;
            themeConfiguration1.CustomHeadStyles = settingByKeyAsync;
            NopTemplatesThemeConfiguration themeConfiguration = themeConfiguration1;
            await this.AddThemeAttributeAsync<string>(themeConfiguration, (Expression<Func<string>>)(() => themeConfiguration.CustomHeadStyles), pluginSystemName, storeId);
            await this._settingsService.DeleteSettingAsync<T, string>((T)themeSettings, (Expression<Func<T, string>>)(x => x.CustomHeadStyles), storeId);
        }

        async Task IGenericAttributeHelper.MovePresetFromSettingsToGenericAttributeAsync(
          string settingKey,
          string pluginSystemName)
        {
            await this.MovePresetFromSettingsToGenericAttributeForStoreAsync(settingKey, pluginSystemName, 0);
            foreach (BaseEntity baseEntity in (IEnumerable<Store>)await this._storeService.GetAllStoresAsync())
            {
                int id = baseEntity.Id;
                await this.MovePresetFromSettingsToGenericAttributeForStoreAsync(settingKey, pluginSystemName, id);
            }
        }

        private async Task MovePresetFromSettingsToGenericAttributeForStoreAsync(
          string settingKey,
          string pluginSystemName,
          int storeId)
        {
            string preset = await this._settingsService.GetSettingByKeyAsync<string>(settingKey, (string)null, storeId, false);
            if (string.IsNullOrEmpty(preset))
            {
                preset = (string)null;
            }
            else
            {
                NopTemplatesThemeConfiguration themeConfiguration = new NopTemplatesThemeConfiguration();
                if (string.IsNullOrEmpty(await this.GetThemeAttributeAsync<string>(themeConfiguration, (Expression<Func<string>>)(() => themeConfiguration.PresetCss), pluginSystemName, storeId, false)))
                {
                    themeConfiguration.PresetCss = this._presetsHelper.GeneratePresetCss(preset, pluginSystemName);
                    await this.AddThemeAttributeAsync<string>(themeConfiguration, (Expression<Func<string>>)(() => themeConfiguration.PresetCss), pluginSystemName, storeId);
                }
                preset = (string)null;
            }
        }


    }
  
  
}
