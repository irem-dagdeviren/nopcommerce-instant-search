// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Plugins.PluginParameter
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using System;
using System.Globalization;

namespace Nop.Plugin.InstantSearch.Dotless.Plugins
{
  public class PluginParameter : IPluginParameter
  {
    public PluginParameter(string name, Type type, bool isMandatory)
    {
      this.Name = name;
      this.IsMandatory = isMandatory;
      this.Type = type;
    }

    public string Name { get; private set; }

    public bool IsMandatory { get; private set; }

    public object Value { get; private set; }

    private Type Type { get; set; }

    public string TypeDescription => this.Type.Name;

    public void SetValue(string stringValue)
    {
      if (this.Type.Equals(typeof (bool)))
      {
        if (stringValue.Equals("true", StringComparison.InvariantCultureIgnoreCase) || stringValue.Equals("t", StringComparison.InvariantCultureIgnoreCase) || stringValue.Equals("1", StringComparison.InvariantCultureIgnoreCase))
          this.Value = (object) true;
        else
          this.Value = (object) false;
      }
      else
        this.Value = Convert.ChangeType((object) stringValue, this.Type, (IFormatProvider) CultureInfo.InvariantCulture);
    }
  }
}
