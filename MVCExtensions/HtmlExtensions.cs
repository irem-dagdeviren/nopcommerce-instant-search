using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Plugin.InstantSearch.Trial;

#nullable enable
namespace Nop.Plugin.InstantSearch.MVCExtensions
{
    public static class HtmlExtensions
    {
        public static Task<string> RenderHtmlContentAsync(IHtmlContent htmlContent)
        {
            throw new NotImplementedException();
        }

        public static async Task<IHtmlContent> TrialMessageAsync(
            this IHtmlHelper htmlHelper,
            bool isTrialVersion,
            string pluginName,
            string pluginUrlInStore)
        {
            return await htmlHelper.TrialMessageAsync(isTrialVersion, pluginName, pluginUrlInStore, (string) null);
        }

        public static async Task<IHtmlContent> TrialMessageAsync(
            this IHtmlHelper htmlHelper,
            bool isTrialVersion,
            string pluginName,
            string pluginUrlInStore,
            string additionalMessage)
        {
            string str = string.Empty;
            if (isTrialVersion)
            str = await HtmlExtensions.RenderHtmlContentAsync(htmlHelper.Partial("~/Plugins/InstantSearch/Areas/Admin/Views/Shared/_TrialAdmin.cshtml", (object) new TrialData()
            {
                PluginName = pluginName,
                PluginUrlInStore = pluginUrlInStore,
                AdditionalMessage = additionalMessage
            }));
            return (IHtmlContent) new HtmlString(str);
        }
    }
}
