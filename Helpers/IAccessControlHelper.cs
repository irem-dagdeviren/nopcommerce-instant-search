namespace Nop.Plugin.InstantSearch.Helpers
{
  public interface IAccessControlHelper
  {
    Task<bool> HasManagePluginsPermissionAsync();

    Task<bool> HasAdminAccessAsync();

    Task<bool> HasManagePluginPermissionAsync(string pluginSystemName);
  }
}
