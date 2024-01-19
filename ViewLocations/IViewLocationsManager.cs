using Microsoft.AspNetCore.Mvc.Razor;

namespace Nop.Plugin.InstantSearch.ViewLocations
{
    public interface IViewLocationsManager : IViewLocationExpander
    {
        IList<DuplicateControllerInfo> DuplicateControllerInfos { get; set; }

        void AddViewLocationFormats(IList<string> viewLocationFormats, bool addFirst);

        void AddAdminViewLocationFormats(IList<string> adminViewLocationFormats, bool addFirst);
    }
}
