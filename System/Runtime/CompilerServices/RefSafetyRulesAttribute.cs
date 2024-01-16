using Microsoft.CodeAnalysis;
using System.Runtime.InteropServices;
using Microsoft.CodeAnalysis.Operations;
namespace System.Runtime.CompilerServices
{
  [CompilerGenerated]
  //[Embedded]
  [AttributeUsage(AttributeTargets.Module, AllowMultiple = false, Inherited = false)]
  internal sealed class RefSafetyRulesAttribute : Attribute
  {
    public readonly int Version;

    public RefSafetyRulesAttribute([In] int obj0) => this.Version = obj0;
  }
}
