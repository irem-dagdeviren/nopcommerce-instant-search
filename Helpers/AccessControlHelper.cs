using Nop.Services.Security;


#nullable enable
namespace Nop.Plugin.InstantSearch.Helpers
{
  public class AccessControlHelper : IAccessControlHelper
  {
    private readonly 
    #nullable disable
    IPermissionService _permissionService;

    public AccessControlHelper(IPermissionService permissionService) => this._permissionService = permissionService;

    public async Task<bool> HasManagePluginsPermissionAsync() => await this._permissionService.AuthorizeAsync(StandardPermissionProvider.ManagePlugins);

    public async Task<bool> HasAdminAccessAsync() => await this._permissionService.AuthorizeAsync(StandardPermissionProvider.AccessAdminPanel);

    public async Task<bool> HasManagePluginPermissionAsync(string pluginSystemName) => await this._permissionService.AuthorizeAsync("Manage" + pluginSystemName);
  }
}
