using System.Reflection;
using MimeKit;

namespace Nop.Plugin.InstantSearch.Dotless.Plugins
{
  public class GenericPluginConfigurator<T> : IPluginConfigurator where T : IPlugin
  {
    private Func<IPlugin> _pluginCreator;

    public string Name => PluginFinder.GetName(typeof (T));

    public string Description => PluginFinder.GetDescription(typeof (T));

    public Type Configurates => typeof (T);

    public void SetParameterValues(IEnumerable<IPluginParameter> pluginParameters)
    {
      ConstructorInfo defaultConstructor;
      this.GetConstructorInfos(out ConstructorInfo _, out defaultConstructor);
      if (pluginParameters == null || pluginParameters.Count<IPluginParameter>() == 0 || pluginParameters.All<IPluginParameter>((Func<IPluginParameter, bool>) (parameter => parameter.Value == null)))
      {
        this._pluginCreator = !(defaultConstructor == (ConstructorInfo) null) ? (Func<IPlugin>) (() => (IPlugin) defaultConstructor.Invoke(new object[0])) : throw new Exception("No parameters provided but no default constructor");
      }
      else
      {
        object[] constructorArguments = ((IEnumerable<ParameterInfo>) GetParameters()).OrderBy<ParameterInfo, int>((Func<ParameterInfo, int>) (parameter => parameter.Position)).Select<ParameterInfo, object>((Func<ParameterInfo, object>) (parameter =>
        {
          IPluginParameter pluginParameter1 = pluginParameters.FirstOrDefault<IPluginParameter>((Func<IPluginParameter, bool>) (pluginParameter => pluginParameter.Name == parameter.Name));
          if (pluginParameter1 != null)
            return pluginParameter1.Value;
          return parameter.ParameterType.IsValueType ? Activator.CreateInstance(parameter.ParameterType) : (object) null;
        })).ToArray<object>();
                this._pluginCreator = (Func<IPlugin>)(() => (IPlugin) defaultConstructor.Invoke(constructorArguments));
      }
    }

    public IPlugin CreatePlugin()
    {
      if (this._pluginCreator == null)
        this.SetParameterValues((IEnumerable<IPluginParameter>) null);
      return this._pluginCreator();
    }

    private void GetConstructorInfos(
      out ConstructorInfo parameterConstructor,
      out ConstructorInfo defaultConstructor)
    {
      List<ConstructorInfo> list = ((IEnumerable<ConstructorInfo>) typeof (T).GetConstructors()).Where<ConstructorInfo>((Func<ConstructorInfo, bool>) (constructorInfo => constructorInfo.IsPublic && !constructorInfo.IsStatic)).ToList<ConstructorInfo>();
      if (list.Count > 2 || list.Count == 0)
        throw new Exception("Generic plugin configurator doesn't support less than 1 or more than 2 constructors. Add your own IPluginConfigurator to the assembly.");
      if (list.Count == 2)
      {
        if (list[0].GetParameters().Length == 0)
        {
          defaultConstructor = list[0];
          parameterConstructor = list[1];
        }
        else
        {
          defaultConstructor = list[1].GetParameters().Length == 0 ? list[1] : throw new Exception("Generic plugin configurator only supports 1 parameterless constructor and 1 with parameters. Add your own IPluginConfigurator to the assembly.");
          parameterConstructor = list[0];
        }
      }
      else if (list[0].GetParameters().Length == 0)
      {
        defaultConstructor = list[0];
        parameterConstructor = (ConstructorInfo) null;
      }
      else
      {
        defaultConstructor = (ConstructorInfo) null;
        parameterConstructor = list[0];
      }
    }

    public IEnumerable<IPluginParameter> GetParameters()
    {
      ConstructorInfo parameterConstructor;
      ConstructorInfo defaultConstructor;
      this.GetConstructorInfos(out parameterConstructor, out defaultConstructor);
      return parameterConstructor == (ConstructorInfo) null ? (IEnumerable<IPluginParameter>) new List<IPluginParameter>() : (IEnumerable<IPluginParameter>) ((IEnumerable<ParameterInfo>) parameterConstructor.GetParameters()).Select<ParameterInfo, IPluginParameter>((Func<ParameterInfo, IPluginParameter>) (parameter => (IPluginParameter) new PluginParameter(parameter.Name, parameter.ParameterType, defaultConstructor == (ConstructorInfo) null))).ToList<IPluginParameter>();
    }

    private class ConstructorParameterSet
    {
      public ParameterInfo[] Parameter { get; set; }

      public int Count { get; set; }
    }
  }
}
