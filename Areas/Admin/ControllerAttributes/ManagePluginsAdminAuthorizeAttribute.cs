using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Nop.Services.Security;
using Nop.Plugin.InstantSearch.Helpers;

namespace Nop.Plugin.InstantSearch.Areas.Admin.ControllerAttributes
{
    public class ManagePluginsAdminAuthorizeAttribute : TypeFilterAttribute
    {
        public ManagePluginsAdminAuthorizeAttribute(
#nullable disable
        string permissionSystemName = "", bool ignore = false)
          : base(typeof(ManagePluginsAdminAuthorizeAttribute.ManagePluginsAdminAuthorizeFilter))
        {
            this.Arguments = new object[2]
            {
        (object) permissionSystemName,
        (object) ignore
            };
        }

        private class ManagePluginsAdminAuthorizeFilter : IAsyncAuthorizationFilter, IFilterMetadata
        {
            private readonly bool _ignoreFilter;
            private readonly string _pluginSystemName;
            private readonly IAccessControlHelper _accessControlHelper;
            private readonly IPermissionService _permissionService;

            public ManagePluginsAdminAuthorizeFilter(
              string pluginSystemName,
              bool ignoreFilter,
              IAccessControlHelper accessControlHelper,
              IPermissionService permissionService)
            {
                this._pluginSystemName = pluginSystemName;
                this._ignoreFilter = ignoreFilter;
                this._accessControlHelper = accessControlHelper;
                this._permissionService = permissionService;
            }

            public async Task OnAuthorizationAsync(AuthorizationFilterContext filterContext)
            {
                if (this._ignoreFilter)
                    return;
                if (filterContext == null)
                    throw new ArgumentNullException(nameof(filterContext));
                if (!await this._accessControlHelper.HasAdminAccessAsync())
                    return;
                bool flag = !string.IsNullOrEmpty(this._pluginSystemName);
                if (flag)
                    flag = !await this._accessControlHelper.HasManagePluginPermissionAsync(this._pluginSystemName);
                if (!flag)
                    return;
                //filterContext.Result = (IActionResult)new RedirectToActionResult("AccessDenied", "Security", (object)filterContext.RouteData.Values);
                return;
            }
        }
    }
}