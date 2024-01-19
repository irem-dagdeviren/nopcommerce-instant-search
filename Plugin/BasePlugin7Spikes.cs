using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Security;
using Nop.Core.Infrastructure;
using Nop.Data;
using Nop.Plugin.InstantSearch.Helpers;
using Nop.Plugin.InstantSearch.Permissions;
using Nop.Services.Localization;
using Nop.Services.Plugins;
using Nop.Services.Security;
using Nop.Web.Framework.Menu;


#nullable enable
namespace Nop.Plugin.InstantSearch.Plugin
{
    public abstract class BasePlugin7Spikes : BasePlugin
    {
        protected string _pluginFolderName;
        private ILocalizationService _localizationService;
        private IWorkContext _workContext;
        private IWebHelper _webHelper;
        private IPermissionService _permissionService;
        private INopFileProvider _nopFileProvider;
        private IAccessControlHelper _accessControlHelper;

        protected ILocalizationService LocalizationService
        {
            get
            {
            if (this._localizationService == null)
                this._localizationService = EngineContext.Current.Resolve<ILocalizationService>((IServiceScope) null);
            return this._localizationService;
            }
        }

        protected IWorkContext WorkContext
        {
            get
            {
            if (this._workContext == null)
                this._workContext = EngineContext.Current.Resolve<IWorkContext>((IServiceScope) null);
            return this._workContext;
            }
        }

        protected IWebHelper WebHelper
        {
            get
            {
            if (this._webHelper == null)
                this._webHelper = EngineContext.Current.Resolve<IWebHelper>((IServiceScope) null);
            return this._webHelper;
            }
        }

        protected IPermissionService PermissionService
        {
            get
            {
            if (this._permissionService == null)
                this._permissionService = EngineContext.Current.Resolve<IPermissionService>((IServiceScope) null);
            return this._permissionService;
            }
        }

        protected INopFileProvider NopFileProvider
        {
            get
            {
            if (this._nopFileProvider == null)
                this._nopFileProvider = EngineContext.Current.Resolve<INopFileProvider>((IServiceScope) null);
            return this._nopFileProvider;
            }
        }

        protected IAccessControlHelper AccessControlHelper
        {
            get
            {
            if (this._accessControlHelper == null)
                this._accessControlHelper = EngineContext.Current.Resolve<IAccessControlHelper>((IServiceScope) null);
            return this._accessControlHelper;
            }
        }

        protected BasePlugin7Spikes(string pluginFolderName) => this._pluginFolderName = pluginFolderName;

        public async Task<bool> AuthenticateAsync()
        {
            bool flag = await this.AccessControlHelper.HasManagePluginsPermissionAsync();
            if (!flag)
            flag = await this.AccessControlHelper.HasManagePluginPermissionAsync(this._pluginFolderName);
            return flag;
        }

        public override  async Task InstallAsync()
        {
            IInstallHelper installHelper = EngineContext.Current.Resolve<IInstallHelper>((IServiceScope) null);
            await installHelper.InstallDefaultPluginSettingsAsync(this._pluginFolderName);
            await installHelper.InstallLocaleResourcesAsync(this._pluginFolderName);
            await this.InstallAdditionalSettingsAsync();
            await this.SetupPermissionRecordsAsync();
            await base.InstallAsync();
            installHelper = (IInstallHelper) null;
        }

        public override async Task UninstallAsync()
        {
            await this.UninstallAdditionalSettingsAsync();
            await this.DeletePluginPermissionRecordAsync();
            await base.UninstallAsync();
        }

        protected virtual async Task InstallAdditionalSettingsAsync() => await Task.CompletedTask;

        protected virtual async Task UninstallAdditionalSettingsAsync() => await Task.CompletedTask;

        protected async Task AttachMenuItemAsync(
            SiteMapNode rootNode,
            string parentNodeSystemName,
            SiteMapNode childMenuItem,
            bool ignorePermissions = false)
        {
            bool flag = !ignorePermissions;
            if (flag)
                flag = !await this.AuthenticateAsync();
            if (flag)
                return;
            else
            {
                SiteMapNode siteMapNode = (await this.EnsureMainMenuInitializedAsync(rootNode)).ChildNodes.FirstOrDefault<SiteMapNode>((Func<SiteMapNode, bool>) (x => x.SystemName == parentNodeSystemName));
                if (siteMapNode == null)
                    return;
                else
            {
                siteMapNode.Visible = true;
                siteMapNode.ChildNodes.Add(childMenuItem);
            }
            }
        }

        private async Task<SiteMapNode> EnsureMainMenuInitializedAsync(SiteMapNode rootNode)
        {
            ILocalizationService ilocalizationService = this.LocalizationService;
            string resourceAsync1 = await ilocalizationService.GetResourceAsync("InstantSearch.Plugin.Admin.Menu.MainMenu", ((BaseEntity) await this.WorkContext.GetWorkingLanguageAsync()).Id, true, "Nop-Templates", false);
            ilocalizationService = (ILocalizationService) null;
            string mainMenuTitle = resourceAsync1;
            ilocalizationService = this.LocalizationService;
            string resourceAsync2 = await ilocalizationService.GetResourceAsync("InstantSearch.Plugin.Admin.Menu.Plugins.MenuName", ((BaseEntity) await this.WorkContext.GetWorkingLanguageAsync()).Id, true, "Plugins", false);
            ilocalizationService = (ILocalizationService) null;
            string pluginsTitle = resourceAsync2;
            ilocalizationService = this.LocalizationService;
            string resourceAsync3 = await ilocalizationService.GetResourceAsync("InstantSearch.Plugin.Admin.Menu.Themes.MenuName", ((BaseEntity) await this.WorkContext.GetWorkingLanguageAsync()).Id, true, "Themes", false);
            ilocalizationService = (ILocalizationService) null;
            string templatesTitle = resourceAsync3;
            ilocalizationService = this.LocalizationService;
            string resourceAsync4 = await ilocalizationService.GetResourceAsync("InstantSearch.Plugin.Admin.Menu.PluginsAccessControl.MenuName", ((BaseEntity) await this.WorkContext.GetWorkingLanguageAsync()).Id, true, "Plugins Access Control", false);
            ilocalizationService = (ILocalizationService) null;
            string accessControlTitle = resourceAsync4;
            ilocalizationService = this.LocalizationService;
            string resourceAsync5 = await ilocalizationService.GetResourceAsync("InstantSearch.Plugin.Admin.Menu.Warnings.MenuName", ((BaseEntity) await this.WorkContext.GetWorkingLanguageAsync()).Id, true, "Warnings", false);
            ilocalizationService = (ILocalizationService) null;
            string warningsTitle = resourceAsync5;
            ilocalizationService = this.LocalizationService;
            string resourceAsync6 = await ilocalizationService.GetResourceAsync("InstantSearch.Plugin.Admin.Menu.Information.MenuName", ((BaseEntity) await this.WorkContext.GetWorkingLanguageAsync()).Id, true, "Information", false);
            ilocalizationService = (ILocalizationService) null;
            string str = resourceAsync6;
            SiteMapNode mainMenuNode = rootNode.ChildNodes.FirstOrDefault<SiteMapNode>((Func<SiteMapNode, bool>) (x => x.SystemName == "nopTemplates"));
            if (mainMenuNode == null)
            {
                mainMenuNode = new SiteMapNode()
                {
                    SystemName = "nopTemplates",
                    Title = mainMenuTitle,
                    RouteValues = new RouteValueDictionary()
                    {
                    {
                        "area",
                        (object) "admin"
                    }
                    },
                    IconClass = "fa icon-nop-templates",
                    Visible = true
                };
                SiteMapNode siteMapNode1 = new SiteMapNode()
                {
                    Title = pluginsTitle,
                    SystemName = "InstantSearch Plugins",
                    Visible = false,
                    IconClass = "fa icon-plugins"
                };
                SiteMapNode siteMapNode2 = new SiteMapNode()
                {
                    Title = templatesTitle,
                    SystemName = "InstantSearch Themes",
                    Visible = false,
                    IconClass = "fa icon-themes"
                };
                SiteMapNode accessControlNode = new SiteMapNode()
                {
                    Title = accessControlTitle,
                    SystemName = "Plugins Access Control",
                    Url = this.WebHelper.GetStoreLocation(new bool?()) + "Admin/NopCoreAdmin/PluginsAccessControl",
                    Visible = true,
                    IconClass = "fa icon-plugins"
                };
                SiteMapNode warningsNode = new SiteMapNode()
                {
                    Title = warningsTitle,
                    SystemName = "InstantSearch Warnings",
                    Url = this.WebHelper.GetStoreLocation(new bool?()) + "Admin/NopCoreAdmin/Warnings",
                    Visible = true,
                    IconClass = "fa fa-exclamation-circle"
                };
                SiteMapNode informationNode = new SiteMapNode()
                {
                    Title = str,
                    SystemName = "InstantSearch Information",
                    Url = this.WebHelper.GetStoreLocation(new bool?()) + "Admin/NopCoreAdmin/Information",
                    Visible = true,
                    IconClass = "fa fa-cube"
                };
                mainMenuNode.ChildNodes.Add(siteMapNode1);
                mainMenuNode.ChildNodes.Add(siteMapNode2);
                if (await this.PermissionService.AuthorizeAsync(StandardPermissionProvider.ManageAcl))
                    mainMenuNode.ChildNodes.Add(accessControlNode);
                if (await this.PermissionService.AuthorizeAsync(StandardPermissionProvider.ManagePlugins))
                {
                    mainMenuNode.ChildNodes.Add(warningsNode);
                    mainMenuNode.ChildNodes.Add(informationNode);
                }
                rootNode.ChildNodes.Add(mainMenuNode);
                accessControlNode = (SiteMapNode) null;
                warningsNode = (SiteMapNode) null;
                informationNode = (SiteMapNode) null;
            }
            SiteMapNode siteMapNode = mainMenuNode;
            mainMenuTitle = (string) null;
            pluginsTitle = (string) null;
            templatesTitle = (string) null;
            accessControlTitle = (string) null;
            warningsTitle = (string) null;
            mainMenuNode = (SiteMapNode) null;
            return siteMapNode;
        }

        protected async Task DeletePluginPermissionRecordAsync()
        {
            string permissionRecordSystemName = "Manage" + this._pluginFolderName;
            PermissionRecord permissionRecord = (await this.PermissionService.GetAllPermissionRecordsAsync()).FirstOrDefault<PermissionRecord>((Func<PermissionRecord, bool>) (x => x.SystemName.Equals(permissionRecordSystemName)));
            if (permissionRecord == null)
                return; 
            else
                await EngineContext.Current.Resolve<IRepository<PermissionRecord>>((IServiceScope) null).DeleteAsync(permissionRecord, true);
        }

        public virtual async Task SetupPermissionRecordsAsync()
        {
            BasePlugin7Spikes basePlugin7Spikes = this;
            Base7SpikesPermissionProvider permissionProvider = new Base7SpikesPermissionProvider(basePlugin7Spikes._pluginFolderName, basePlugin7Spikes.PluginDescriptor.FriendlyName, NopCustomerDefaults.AdministratorsRoleName);
            await basePlugin7Spikes.PermissionService.InstallPermissionsAsync((IPermissionProvider) permissionProvider);
        }

        public override abstract  string GetConfigurationPageUrl();
    }
}
