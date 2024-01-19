using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Domain.Stores;
using Nop.Core.Infrastructure;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Stores;
using Nop.Web.Areas.Admin.Controllers;
using Nop.Plugin.InstantSearch.Areas.Admin.ControllerAttributes;
using Nop.Plugin.InstantSearch.Areas.Admin.Models;


#nullable enable
namespace Nop.Plugin.InstantSearch.Controllers
{
    [ManagePluginsAdminAuthorize("", false)]
    public abstract class Base7SpikesAdminController : BaseAdminController
    {
    private IStoreContext _storeContext;
    private INotificationService _notificationService;

    protected IStoreContext StoreContext
    {
        get
        {
        if (this._storeContext == null)
            this._storeContext = EngineContext.Current.Resolve<IStoreContext>((IServiceScope) null);
        return this._storeContext;
        }
    }

    private INotificationService NotificationService
    {
        get
        {
        if (this._notificationService == null)
            this._notificationService = EngineContext.Current.Resolve<INotificationService>((IServiceScope) null);
        return this._notificationService;
        }
    }

    protected void SuccessNotification(string message) => this.NotificationService.SuccessNotification(message, true);

    }
}
