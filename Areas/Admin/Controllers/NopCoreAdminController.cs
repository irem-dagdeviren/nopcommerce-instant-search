using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Security;
using Nop.Core.Infrastructure;
using Nop.Data;
using Nop.Services.Customers;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Plugins;
using Nop.Services.Security;
using Nop.Web.Areas.Admin.Controllers;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.Common;
using Nop.Web.Areas.Admin.Models.Customers;
using Nop.Web.Areas.Admin.Models.Security;
using Nop.Web.Framework.Controllers;
using Nop.Plugin.InstantSearch;
using Nop.Plugin.InstantSearch.Areas.Admin.Models;
using Nop.Plugin.InstantSearch.Plugin;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using MailKit;


#nullable enable
namespace Nop.Plugin.InstantSearch.Areas.Admin.Controllers
{
  public class NopCoreAdminController : BaseAdminController
  {
    private readonly 
    #nullable disable
    IPluginService _pluginService;
    private readonly ICustomerService _customerService;
    private readonly IPermissionService _permissionService;
    private readonly ILocalizationService _localizationService;
    private readonly INotificationService _notificationService;
    private readonly IRepository<PermissionRecord> _permissionRecordRepository;

    public NopCoreAdminController(
      IPluginService pluginService,
      ICustomerService customerService,
      IPermissionService permissionService,
      ILocalizationService localizationService,
      INotificationService notificationService,
      IRepository<PermissionRecord> permissionRecordRepository)
    {
      this._pluginService = pluginService;
      this._customerService = customerService;
      this._permissionService = permissionService;
      this._localizationService = localizationService;
      this._notificationService = notificationService;
      this._permissionRecordRepository = permissionRecordRepository;
    }

    public async Task<ActionResult> Warnings()
    {
      NopCoreAdminController coreAdminController = this;
      IList<WarningsModel> model = (IList<WarningsModel>) new List<WarningsModel>();
      foreach (BaseAdminPlugin7Spikes adminPlugin7Spikes in (IEnumerable<BaseAdminPlugin7Spikes>) await coreAdminController._pluginService.GetPluginsAsync<BaseAdminPlugin7Spikes>((LoadPluginsMode) 10, (Customer) null, 0, (string) null))
      {
        WarningsModel warningsModel = await adminPlugin7Spikes.WarningsAsync();
        if (warningsModel.Warnings.Count > 0)
          model.Add(warningsModel);
      }
      ActionResult actionResult = (ActionResult) ((Controller) coreAdminController).View("~/Plugins/InstantSearch/Areas/Admin/Views/NopCoreAdmin/Warnings.cshtml", (object) model);
      model = (IList<WarningsModel>) null;
      return actionResult;
    }

    public async Task<ActionResult> ResetDefaultSettings(string pluginFolderName, string returnUrl)
    {
      NopCoreAdminController coreAdminController = this;
      await EngineContext.Current.Resolve<IInstallHelper>((IServiceScope) null).InstallDefaultPluginSettingsAsync(pluginFolderName);
      return !string.IsNullOrEmpty(returnUrl) ? (((ControllerBase) coreAdminController).Url.IsLocalUrl(returnUrl) ? (ActionResult) ((ControllerBase) coreAdminController).Redirect(returnUrl) : (ActionResult) ((ControllerBase) coreAdminController).RedirectToAction("Index", "Home", (object) new
      {
        area = "Admin"
      })) : (ActionResult) ((ControllerBase) coreAdminController).RedirectToAction("Index", "Home", (object) new
      {
        area = "Admin"
      });
    }

        public ActionResult Information()
        {
            List<SystemInfoModel.LoadedAssembly> model = new List<SystemInfoModel.LoadedAssembly>();
            foreach (Assembly assembly in ((IEnumerable<Assembly>)AppDomain.CurrentDomain.GetAssemblies()).Where<Assembly>((Func<Assembly, bool>)(x => x.FullName.Contains("InstantSearch"))).OrderByDescending<Assembly, string>((Func<Assembly, string>)(x => ((IEnumerable<string>)x.GetName().Version.ToString().Split('.')).LastOrDefault<string>())).ThenBy<Assembly, string>((Func<Assembly, string>)(x => x.FullName)).ToList<Assembly>())
            {
                SystemInfoModel.LoadedAssembly loadedAssembly = new SystemInfoModel.LoadedAssembly()
                {
                    FullName = assembly.FullName
                };
                try
                {
                    loadedAssembly.Location = assembly.IsDynamic ? (string)null : assembly.Location;
                    loadedAssembly.IsDebug = this.IsDebugAssembly(assembly);
                    loadedAssembly.BuildDate = assembly.IsDynamic ? new DateTime?() : new DateTime?(TimeZoneInfo.ConvertTimeFromUtc(System.IO.File.GetLastWriteTimeUtc(assembly.Location), TimeZoneInfo.Local));
                }
                catch (Exception ex)
                {
                }
                model.Add(loadedAssembly);
            }
            return (ActionResult)((Controller)this).View("~/Plugins/InstantSearch/Areas/Admin/Views/NopCoreAdmin/Information.cshtml", (object)model);
        }

        public async Task<IActionResult> PluginsAccessControl()
    {
      NopCoreAdminController coreAdminController = this;
      if (!await coreAdminController._permissionService.AuthorizeAsync(StandardPermissionProvider.ManageAcl))
        return ((NopCoreAdminController) coreAdminController).AccessDeniedView();
      PermissionMappingModel model = new PermissionMappingModel();
      IList<CustomerRole> customerRoles = await coreAdminController._customerService.GetAllCustomerRolesAsync(true);
      model.AvailableCustomerRoles = (IList<CustomerRoleModel>) customerRoles.Select<CustomerRole, CustomerRoleModel>((Func<CustomerRole, CustomerRoleModel>) (role => MappingExtensions.ToModel<CustomerRoleModel>((BaseEntity) role))).ToList<CustomerRoleModel>();
      foreach (PermissionRecord permissionRecord in (IEnumerable<PermissionRecord>) await coreAdminController.GetAllPermissionRecordsAsync())
      {
        IList<PermissionRecordModel> permissionRecordModelList = model.AvailablePermissions;
        PermissionRecordModel permissionRecordModel1 = new PermissionRecordModel();
        PermissionRecordModel permissionRecordModel2 = permissionRecordModel1;
        permissionRecordModel2.Name = await coreAdminController._localizationService.GetLocalizedPermissionNameAsync(permissionRecord, new int?());
        permissionRecordModel1.SystemName = permissionRecord.SystemName;
        permissionRecordModelList.Add(permissionRecordModel1);
        permissionRecordModelList = (IList<PermissionRecordModel>) null;
        permissionRecordModel2 = (PermissionRecordModel) null;
        permissionRecordModel1 = (PermissionRecordModel) null;
        foreach (CustomerRole customerRole in (IEnumerable<CustomerRole>) customerRoles)
        {
          CustomerRole role = customerRole;
          if (!model.Allowed.ContainsKey(permissionRecord.SystemName))
            model.Allowed[permissionRecord.SystemName] = (IDictionary<int, bool>) new Dictionary<int, bool>();
          IDictionary<int, bool> dictionary = model.Allowed[permissionRecord.SystemName];
          int id = ((BaseEntity) role).Id;
          dictionary[id] = (await coreAdminController._permissionService.GetMappingByPermissionRecordIdAsync(((BaseEntity) permissionRecord).Id)).Any<PermissionRecordCustomerRoleMapping>((Func<PermissionRecordCustomerRoleMapping, bool>) 
              (mapping => mapping.CustomerRoleId == ((BaseEntity) role).Id));
          dictionary = (IDictionary<int, bool>) null;
        }
      }
      return (IActionResult) ((Controller) coreAdminController).View("~/Plugins/InstantSearch/Areas/Admin/Views/NopCoreAdmin/AccessControl.cshtml", (object) model);
    }

    [HttpPost]
    [ActionName("PluginsAccessControl")]
    public async Task<IActionResult> PluginsAccessControlSave(
      PermissionMappingModel model,
      IFormCollection form)
    {
      NopCoreAdminController coreAdminController = this;
      if (!await coreAdminController._permissionService.AuthorizeAsync(StandardPermissionProvider.ManageAcl))
        return ((NopCoreAdminController) coreAdminController).AccessDeniedView();
      IList<PermissionRecord> permissionRecords7Spikes = await coreAdminController.GetAllPermissionRecordsAsync();
      foreach (CustomerRole customerRole in (IEnumerable<CustomerRole>) await coreAdminController._customerService.GetAllCustomerRolesAsync(true))
      {
        CustomerRole cr = customerRole;
        string key = "allow_" + ((BaseEntity) cr).Id.ToString();
        List<string> stringList;
        if (StringValues.IsNullOrEmpty(form[key]))
          stringList = new List<string>();
        else
          stringList = ((IEnumerable<string>) form[key].ToString().Split(new char[1]
          {
            ','
          }, StringSplitOptions.RemoveEmptyEntries)).ToList<string>();
        List<string> permissionRecordSystemNamesToRestrict = stringList;
        foreach (PermissionRecord pr in (IEnumerable<PermissionRecord>) permissionRecords7Spikes)
        {
          bool allow = permissionRecordSystemNamesToRestrict.Contains(pr.SystemName);
          IList<PermissionRecordCustomerRoleMapping> permissionRecordIdAsync = await coreAdminController._permissionService.GetMappingByPermissionRecordIdAsync(((BaseEntity) pr).Id);
          if (allow)
          {
            if (permissionRecordIdAsync.FirstOrDefault<PermissionRecordCustomerRoleMapping>((Func<PermissionRecordCustomerRoleMapping, bool>) (x => x.CustomerRoleId == ((BaseEntity) cr).Id)) == null)
              await coreAdminController._permissionService.InsertPermissionRecordCustomerRoleMappingAsync(new PermissionRecordCustomerRoleMapping()
              {
                PermissionRecordId = ((BaseEntity) pr).Id,
                CustomerRoleId = ((BaseEntity) cr).Id
              });
            else
              continue;
          }
          else if (permissionRecordIdAsync.FirstOrDefault<PermissionRecordCustomerRoleMapping>((Func<PermissionRecordCustomerRoleMapping, bool>) (x => x.CustomerRoleId == ((BaseEntity) cr).Id)) != null)
            await coreAdminController._permissionService.DeletePermissionRecordCustomerRoleMappingAsync(((BaseEntity) pr).Id, ((BaseEntity) cr).Id);
          else
            continue;
          await coreAdminController._permissionService.UpdatePermissionRecordAsync(pr);
        }
        permissionRecordSystemNamesToRestrict = (List<string>) null;
      }
      INotificationService inotificationService = coreAdminController._notificationService;
      inotificationService.SuccessNotification(await coreAdminController._localizationService.GetResourceAsync("Admin.Configuration.ACL.Updated"), true);
      inotificationService = (INotificationService) null;
      return (IActionResult) ((ControllerBase) coreAdminController).RedirectToAction("PluginsAccessControl");
    }

    private bool IsDebugAssembly(Assembly assembly)
    {
      object[] customAttributes = assembly.GetCustomAttributes(typeof (DebuggableAttribute), false);
      return customAttributes.Length != 0 && customAttributes[0] is DebuggableAttribute debuggableAttribute && debuggableAttribute.IsJITOptimizerDisabled;
    }

    private async Task<IList<PermissionRecord>> GetAllPermissionRecordsAsync()
    {
            ParameterExpression parameterExpression1 = null;
            ParameterExpression parameterExpression2 = null;

       var result = await this._permissionRecordRepository.Table
    .Where(record => record.SystemName.Contains("InstantSearch"))
    .OrderBy(record => record.Name)
    .ToListAsync();

            return (IList<PermissionRecord>)result;
        }
    }
}
