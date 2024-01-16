using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System.Runtime.CompilerServices;


#nullable enable
namespace Nop.Plugin.InstantSearch.Areas.Admin.Models
{
  public record CopyModel() : BaseNopEntityModel
  {
    [NopResourceDisplayName("Nop.Plugin.Admin.Copy.Name")]
    public 
    #nullable disable
    string Name { get; set; }

    public bool SupportCopyConditions { get; set; }

    [NopResourceDisplayName("Nop.Plugin.Admin.Copy.CopyConditions")]
    public bool CopyConditions { get; set; }

    public bool SupportCopyMappings { get; set; }

    [NopResourceDisplayName("Nop.Plugin.Admin.Copy.CopyMappings")]
    public bool CopyMappings { get; set; }

    public bool SupportCopyScheduling { get; set; }

    [NopResourceDisplayName("Nop.Plugin.Admin.Copy.CopyScheduling")]
    public bool CopyScheduling { get; set; }

    public string ActionName { get; set; }

    public string ControllerName { get; set; }


    [CompilerGenerated]
    public virtual bool Equals(CopyModel? other)
    {
      if ((object) this == (object) other)
        return true;
 
      return base.Equals((BaseNopEntityModel) other) && EqualityComparer<string>.Default.Equals(this.Name, other.Name) && EqualityComparer<bool>.Default.Equals(this.SupportCopyConditions, other.SupportCopyConditions) && EqualityComparer<bool>.Default.Equals(this.CopyConditions, other.CopyConditions) && EqualityComparer<bool>.Default.Equals(this.SupportCopyMappings, other.SupportCopyMappings) && EqualityComparer<bool>.Default.Equals(this.CopyMappings, other.CopyMappings) && EqualityComparer<bool>.Default.Equals(this.SupportCopyScheduling, other.SupportCopyScheduling) && EqualityComparer<bool>.Default.Equals(this.CopyScheduling, other.CopyScheduling) && EqualityComparer<string>.Default.Equals(this.ActionName, other.ActionName) && EqualityComparer<string>.Default.Equals(this.ControllerName, other.ControllerName);
    }

    [CompilerGenerated]
    protected CopyModel(CopyModel original)
      : base((BaseNopEntityModel) original)
    {
            this.Name = original.Name;
            this.SupportCopyConditions = original.SupportCopyConditions;
            this.CopyConditions = original.CopyConditions;
            this.SupportCopyMappings = original.SupportCopyMappings;
            this.CopyMappings = original.CopyMappings;
            this.SupportCopyScheduling = original.SupportCopyScheduling;
            this.CopyScheduling = original.CopyScheduling;
            this.ActionName = original.ActionName;
            this.ControllerName = original.ControllerName;
    }
  }
}
