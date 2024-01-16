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

    [CompilerGenerated]
    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(nameof (WarningsModel));
      stringBuilder.Append(" { ");
      if (base.PrintMembers(stringBuilder))
        stringBuilder.Append(' ');
      stringBuilder.Append('}');
      return stringBuilder.ToString();
    }

    [CompilerGenerated]
    protected override bool PrintMembers(StringBuilder builder)
    {
      if (base.PrintMembers(builder))
        builder.Append(", ");
      builder.Append("PluginName = ");
      builder.Append((object) this.PluginName);
      builder.Append(", Warnings = ");
      builder.Append((object) this.Warnings);
      return true;
    }

    [CompilerGenerated]
    public override int GetHashCode() => (base.GetHashCode() * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.PluginName)) * -1521134295 + EqualityComparer<IList<SystemWarningModel>>.Default.GetHashCode(this.Warnings);

    
    [CompilerGenerated]
    public virtual bool Equals(WarningsModel other)
    {
      if ((object) this == (object) other)
        return true;
      return base.Equals((BaseNopModel) other) && EqualityComparer<string>.Default.Equals(this.PluginName, other.PluginName) && EqualityComparer<IList<SystemWarningModel>>.Default.Equals(this.Warnings, other.Warnings);
    }

    [CompilerGenerated]
    protected WarningsModel(WarningsModel original)
      : base((BaseNopModel) original)
    {
        this.PluginName = original.PluginName;
        this.Warnings = original.Warnings;
    }
  }
}
