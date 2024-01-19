using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;
using Nop.Plugin.InstantSearch.ActionFilters;
using Nop.Plugin.InstantSearch.Helpers;
using Nop.Plugin.InstantSearch.Services.Catalog;
using Nop.Plugin.InstantSearch.Services.Helpers;
using Nop.Plugin.InstantSearch.ViewLocations;
using Nop.Services.Media;
using Nop.Services.Configuration;
using Nop.Plugin.InstantSearch.Factories;

namespace Nop.Plugin.InstantSearch.Infrastructure
{
    public class NopStartup : INopStartup
    {
        public int Order => 3000;
        public void Configure(IApplicationBuilder application)
        {
        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IInstallHelper, InstallHelper>();
            services.AddScoped<IViewLocationsManager, ViewLocationsManager>();
            services.AddScoped<IAccessControlHelper, AccessControlHelper>();
            services.AddScoped<GlobalActionFiltersProvider>();
            services.AddScoped<ICategoryService7Spikes, CategoryService7Spikes>();
            services.AddScoped<IAclHelper, AclHelper>();
            services.AddScoped<IPictureService, PictureService>();
            services.AddScoped<ISettingService, SettingService>();
            services.AddScoped<IInstantSearchProductModelFactory, InstantSearchProductModelFactory>();
        }
    }
}
