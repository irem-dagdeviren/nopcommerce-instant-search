using Nop.Web.Areas.Admin.Models.Common;
using Nop.Web.Framework.Models;
using System.Runtime.CompilerServices;
using System.Text;


#nullable enable
namespace Nop.Plugin.InstantSearch.Areas.Admin.Models
{
  public record WarningsModel : BaseNopModel
  {
    public WarningsModel() => this.Warnings = (IList<SystemWarningModel>) new List<SystemWarningModel>();

    public string PluginName { get; set; }

    public IList<SystemWarningModel> Warnings { get; set; }

  }
}
