﻿using Microsoft.Extensions.DependencyInjection;
using Nop.Core;
using Nop.Core.Domain.Localization;
using Nop.Core.Infrastructure;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Plugins;
using Nop.Web.Framework.Menu;


#nullable enable
namespace Nop.Plugin.InstantSearch.Plugin
{
    public abstract class BaseAdminPlugin7Spikes : BasePlugin7Spikes, IAdminMenuPlugin, IPlugin
    {
        private readonly 
        #nullable disable
        List<MenuItem7Spikes> _menuItems;
        private readonly string _pluginAdminMenuResourceKey;
        private readonly bool _isTrialVersion;
        private readonly string _pluginUrlInStore;

        protected string StoreLocation => EngineContext.Current.Resolve<IWebHelper>((IServiceScope) null).GetStoreLocation(new bool?());

        protected BaseAdminPlugin7Spikes(
            List<MenuItem7Spikes> menuItems,
            string pluginAdminMenuResourceKey,
            string pluginFolderName,
            bool isTrialVersion = false,
            string pluginUrlInStore = "")
            : base(pluginFolderName)
        {
            this._menuItems = menuItems;
            this._pluginAdminMenuResourceKey = pluginAdminMenuResourceKey;
            this._isTrialVersion = isTrialVersion;
            this._pluginUrlInStore = pluginUrlInStore;
        }

        public abstract override string GetConfigurationPageUrl();

        private async Task<string> GetResourceAsync(string resourceName)
        {
            string resourceAsync = await this.LocalizationService.GetResourceAsync(resourceName);
            return !string.IsNullOrEmpty(resourceAsync) ? resourceAsync : resourceName;
        }

        public virtual async Task<SiteMapNode> BuildMenuItemAsync()
        {
            BaseAdminPlugin7Spikes adminPlugin7Spikes = this;
            ILocalizationService ilocalizationService = adminPlugin7Spikes.LocalizationService;
            string str = adminPlugin7Spikes._pluginAdminMenuResourceKey;
            LocaleStringResource resourceByNameAsync = await ilocalizationService.GetLocaleStringResourceByNameAsync(str, ((BaseEntity) await adminPlugin7Spikes.WorkContext.GetWorkingLanguageAsync()).Id, true);
            ilocalizationService = (ILocalizationService) null;
            str = (string) null;
            LocaleStringResource localeStringResource = resourceByNameAsync;
            string pluginMenuName = adminPlugin7Spikes._pluginAdminMenuResourceKey;
            if (localeStringResource != null)
                pluginMenuName = localeStringResource.ResourceValue;
            SiteMapNode siteMapNode1 = new SiteMapNode();
            siteMapNode1.Title = pluginMenuName;
            SiteMapNode siteMapNode2 = siteMapNode1;
            siteMapNode2.Visible = await adminPlugin7Spikes.AuthenticateAsync();
            siteMapNode1.SystemName = adminPlugin7Spikes._pluginFolderName;
            siteMapNode1.IconClass = "far fa-dot-circle";
            SiteMapNode nextMenuNode = siteMapNode1;
            siteMapNode2 = (SiteMapNode) null;
            siteMapNode1 = (SiteMapNode) null;
            IList<SiteMapNode> siteMapNodeList;
            foreach (MenuItem7Spikes menuItem1 in adminPlugin7Spikes._menuItems)
            {
                MenuItem7Spikes menuItem = menuItem1;
                siteMapNodeList = nextMenuNode.ChildNodes;
                siteMapNode2 = new SiteMapNode();
                siteMapNode1 = siteMapNode2;
                siteMapNode1.Title = await adminPlugin7Spikes.GetResourceAsync(menuItem.SubMenuName);
                siteMapNode2.Url = adminPlugin7Spikes.WebHelper.GetStoreLocation(new bool?()) + "admin/" + menuItem.SubMenuRelativePath;
                siteMapNode2.Visible = true;
                siteMapNode2.SystemName = menuItem.SubMenuName;
                siteMapNode2.IconClass = "far fa-circle";
                siteMapNodeList.Add(siteMapNode2);
                siteMapNodeList = (IList<SiteMapNode>) null;
                siteMapNode1 = (SiteMapNode) null;
                siteMapNode2 = (SiteMapNode) null;
                menuItem = new MenuItem7Spikes();
            }
            if (adminPlugin7Spikes._isTrialVersion && !string.IsNullOrEmpty(adminPlugin7Spikes._pluginUrlInStore))
            nextMenuNode.ChildNodes.Add(new SiteMapNode()
            {
                Title = "BUY FULL VERSION",
                Url = adminPlugin7Spikes._pluginUrlInStore,
                Visible = true,
                SystemName = string.Format("Trial_{0}", (object) adminPlugin7Spikes._pluginFolderName),
                IconClass = "far fa-circle"
            });
            if (!await EngineContext.Current.Resolve<ISettingService>((IServiceScope) null).GetSettingByKeyAsync<bool>("InstantSearch.HidePluginHelpMenu", false, 0, false))
            {
                str = "http://www.nop-templates.com/documentation";
                if (!string.IsNullOrEmpty(adminPlugin7Spikes._pluginUrlInStore))
                    str = string.Format("{0}-documentation", (object) adminPlugin7Spikes._pluginUrlInStore);
                siteMapNodeList = nextMenuNode.ChildNodes;
                siteMapNode1 = new SiteMapNode();
                siteMapNode2 = siteMapNode1;
                siteMapNode2.Title = await adminPlugin7Spikes.LocalizationService.GetResourceAsync("InstantSearch.Admin.Help");
                siteMapNode1.Url = str;
                siteMapNode1.Visible = true;
                siteMapNode1.SystemName = string.Format("{0} Help", (object) pluginMenuName);
                siteMapNode1.IconClass = "far fa-circle";
                siteMapNode1.OpenUrlInNewTab = true;
                siteMapNodeList.Add(siteMapNode1);
                siteMapNodeList = (IList<SiteMapNode>) null;
                siteMapNode2 = (SiteMapNode) null;
                siteMapNode1 = (SiteMapNode) null;
                str = (string) null;
            }
            SiteMapNode siteMapNode = nextMenuNode;
            pluginMenuName = (string) null;
            nextMenuNode = (SiteMapNode) null;
            return siteMapNode;
        }

        public virtual async Task ManageSiteMapAsync(SiteMapNode rootNode)
        {
            BaseAdminPlugin7Spikes adminPlugin7Spikes = this;
            if (!await adminPlugin7Spikes.AuthenticateAsync())
                return;
            SiteMapNode childMenuItem = await adminPlugin7Spikes.BuildMenuItemAsync();
            if (childMenuItem == null)
                return;
            await adminPlugin7Spikes.AttachMenuItemAsync(rootNode, adminPlugin7Spikes.ParentNodeSystemName, childMenuItem);
        }

        protected virtual string ParentNodeSystemName => "InstantSearch Plugins";
    }
}
