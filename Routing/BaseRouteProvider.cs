using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Domain.Localization;
using Nop.Core.Infrastructure;
using Nop.Data;
using Nop.Services.Localization;
using Nop.Web.Framework.Mvc.Routing;
using Nop.Plugin.InstantSearch.ActionFilters;
using Nop.Plugin.InstantSearch.ViewLocations;


namespace Nop.Plugin.InstantSearch.Routing
{
  public abstract class BaseRouteProvider : IRouteProvider
  {
    private readonly bool _shouldRegisterPluginViewLocations;

    private IViewLocationsManager ViewLocationsManager => EngineContext.Current.Resolve<IViewLocationsManager>((IServiceScope) null);

    private GlobalActionFiltersProvider GlobalFiltersProvider7Spikes => EngineContext.Current.Resolve<GlobalActionFiltersProvider>((IServiceScope) null);

    protected abstract string PluginSystemName { get; }

    protected BaseRouteProvider(bool shouldRegisterPluginViewLocations = true) => this._shouldRegisterPluginViewLocations = shouldRegisterPluginViewLocations;

    public void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder)
    {
      this.InstallMissingDefaultSettingsAsync().Wait();
      this.InstallMissingDefaultEntitySettingsAsync().Wait();
      this.UpdateDatabaseAsync().Wait();
      this.CheckBackwardCompatibilityAsync().Wait();
      this.InstallMissingMessageTemplatesAsync().Wait();
      this.InstallMissingResourcesAsync().Wait();
      this.SetNopcommerceSettingsAsync().Wait();
      this.RegisterRoutesAccessibleByNameAsync(endpointRouteBuilder).Wait();
      this.RegisterDuplicateControllers(this.ViewLocationsManager);
      this.RegisterPluginViewLocations(this.ViewLocationsManager);
      this.RegisterPluginActionFilters((IList<IFilterProvider>) this.GlobalFiltersProvider7Spikes.Providers);
    }

    public virtual int Priority => 0;

    protected async Task<string> GetRouteLanguagePatternAsync(
      IEndpointRouteBuilder endpointRouteBuilder)
    {
      string languagePatternAsync = string.Empty;
      if (DataSettingsManager.IsDatabaseInstalled() && endpointRouteBuilder.ServiceProvider.GetRequiredService<LocalizationSettings>().SeoFriendlyUrlsForLanguagesEnabled)
        languagePatternAsync = "{language:lang=" + (await endpointRouteBuilder.ServiceProvider.GetRequiredService<ILanguageService>().GetAllLanguagesAsync(false, 0)).ToList<Language>().FirstOrDefault<Language>().UniqueSeoCode + "}/";
      return languagePatternAsync;
    }

    protected async Task<string> GetSeNameRouteLanguagePatternAsync(
      IEndpointRouteBuilder endpointRouteBuilder)
    {
      string languagePatternAsync = "{SeName}";
      if (DataSettingsManager.IsDatabaseInstalled() && endpointRouteBuilder.ServiceProvider.GetRequiredService<LocalizationSettings>().SeoFriendlyUrlsForLanguagesEnabled)
        languagePatternAsync = "{language:lang=" + (await endpointRouteBuilder.ServiceProvider.GetRequiredService<ILanguageService>().GetAllLanguagesAsync(false, 0)).ToList<Language>().FirstOrDefault<Language>().UniqueSeoCode + "}/{SeName}";
      return languagePatternAsync;
    }

    protected virtual async Task InstallMissingDefaultEntitySettingsAsync() => await Task.CompletedTask;

    protected virtual async Task InstallMissingMessageTemplatesAsync() => await Task.CompletedTask;

    protected virtual async Task InstallMissingResourcesAsync()
    {
      if (string.IsNullOrEmpty(this.PluginSystemName))
        return;
      await EngineContext.Current.Resolve<IInstallHelper>((IServiceScope) null).InstallLocaleResourcesAsync(this.PluginSystemName, updateExistingResources: false);
    }

    protected virtual async Task CheckBackwardCompatibilityAsync() => await Task.CompletedTask;

    protected virtual async Task SetNopcommerceSettingsAsync() => await Task.CompletedTask;

    protected virtual async Task UpdateDatabaseAsync() => await Task.CompletedTask;

    protected virtual async Task RegisterRoutesAccessibleByNameAsync(
      IEndpointRouteBuilder endpointRouteBuilder)
    {
      await Task.CompletedTask;
    }

    protected virtual void RegisterPluginActionFilters(IList<IFilterProvider> providers)
    {
    }

    protected virtual void RegisterDuplicateControllers(IViewLocationsManager viewLocationsManager)
    {
    }

    protected virtual void RegisterCustomViewEngine()
    {
    }

    protected virtual IList<string> GetPluginViewLocations() => (IList<string>) new List<string>()
    {
      "~/Plugins/" + this.PluginSystemName + "/Theme/{2}/Views/{1}/{0}.cshtml",
      "~/Plugins/" + this.PluginSystemName + "/Theme/{2}/Views/{0}.cshtml",
      "~/Plugins/" + this.PluginSystemName + "/Views/{1}/{0}.cshtml",
      "~/Plugins/" + this.PluginSystemName + "/Views/{0}.cshtml"
    };

    protected virtual IList<string> GetPluginAdminViewLocations() => (IList<string>) new List<string>()
    {
        "~/Plugins/InstantSearch/Areas/Admin/Views/InstantSearchAdmin/Settings.cshtml",
      "~/Plugins/" + this.PluginSystemName + "/Areas/Admin/Views/{1}/{0}.cshtml",
      "~/Plugins/" + this.PluginSystemName + "/Areas/Admin/Views/{0}.cshtml"
    };

    protected virtual IList<IControllerActionFilterFactory> GetRollOverActionFilterFactories() => (IList<IControllerActionFilterFactory>) new List<IControllerActionFilterFactory>();

    protected virtual bool ShouldAddPluginViewLocationsBeforeNopViewLocations() => false;

    protected virtual bool ShouldAddPluginAdminViewLocationsBeforeNopViewLocations() => false;

    private async Task InstallMissingDefaultSettingsAsync()
    {
      if (string.IsNullOrEmpty(this.PluginSystemName))
        return;
      await EngineContext.Current.Resolve<IInstallHelper>((IServiceScope) null).InstallDefaultPluginSettingsAsync(this.PluginSystemName, false);
    }

    private void RegisterPluginViewLocations(IViewLocationsManager viewLocationsManager)
    {
      if (!this._shouldRegisterPluginViewLocations)
        return;
      IList<string> pluginViewLocations = this.GetPluginViewLocations();
      bool addFirst1 = this.ShouldAddPluginViewLocationsBeforeNopViewLocations();
      viewLocationsManager.AddViewLocationFormats(pluginViewLocations, addFirst1);
      bool addFirst2 = this.ShouldAddPluginAdminViewLocationsBeforeNopViewLocations();
      IList<string> adminViewLocations = this.GetPluginAdminViewLocations();
      viewLocationsManager.AddAdminViewLocationFormats(adminViewLocations, addFirst2);
    }
  }
}
