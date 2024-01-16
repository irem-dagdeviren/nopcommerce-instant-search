using Microsoft.AspNetCore.Mvc.Razor;

namespace Nop.Plugin.InstantSearch.ViewLocations
{
  public class ThemeablePluginViewLocationExpander : IViewLocationExpander
  {
    public void PopulateValues(ViewLocationExpanderContext context) => (context.ActionContext.HttpContext.RequestServices.GetService(typeof (IViewLocationsManager)) as IViewLocationExpander).PopulateValues(context);

    public IEnumerable<string> ExpandViewLocations(
      ViewLocationExpanderContext context,
      IEnumerable<string> viewLocations)
    {
      return (context.ActionContext.HttpContext.RequestServices.GetService(typeof (IViewLocationsManager)) as IViewLocationExpander).ExpandViewLocations(context, viewLocations);
    }
  }
}
