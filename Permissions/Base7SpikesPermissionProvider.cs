using Nop.Core.Domain.Security;
using Nop.Services.Security;

namespace Nop.Plugin.InstantSearch.Permissions
{
  public class Base7SpikesPermissionProvider : IPermissionProvider
  {
    private string _pluginFolderName;
    private string _pluginFriendlyName;
    private string _systemRoleName;

    public Base7SpikesPermissionProvider(
      string pluginFolderName,
      string pluginFriendlyName,
      string systemRoleName)
    {
      this._pluginFolderName = pluginFolderName;
      this._pluginFriendlyName = pluginFriendlyName;
      this._systemRoleName = systemRoleName;
    }

    public HashSet<(string systemRoleName, PermissionRecord[] permissions)> GetDefaultPermissions() => new HashSet<(string, PermissionRecord[])>()
    {
      (this._systemRoleName, this.GetPermissionRecordsArray())
    };

    public IEnumerable<PermissionRecord> GetPermissions() => (IEnumerable<PermissionRecord>) this.GetPermissionRecordsArray();

    private PermissionRecord[] GetPermissionRecordsArray() => new PermissionRecord[1]
    {
      new PermissionRecord()
      {
        Name = "Admin area. Manage " + this._pluginFriendlyName + " plugin",
        SystemName = "Manage" + this._pluginFolderName,
        Category = "Plugin Configuration"
      }
    };
  }
}
