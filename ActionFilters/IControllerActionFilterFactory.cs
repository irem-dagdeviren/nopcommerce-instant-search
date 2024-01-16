using Microsoft.AspNetCore.Mvc.Filters;

namespace Nop.Plugin.InstantSearch.ActionFilters
{
  public interface IControllerActionFilterFactory
  {
    string ControllerName { get; }

    string ActionName { get; }

    ActionFilterAttribute GetActionFilterAttribute();
  }
}
