using Nop.Plugin.InstantSearch.Dotless.Input;
using Nop.Plugin.InstantSearch.Dotless.Loggers;
using Nop.Plugin.InstantSearch.Dotless.Plugins;
using System;
using System.Collections.Generic;

namespace Nop.Plugin.InstantSearch.Dotless.configuration
{
  public class DotlessConfiguration
  {
    public const string DEFAULT_SESSION_QUERY_PARAM_NAME = "sstate";
    public const int DefaultHttpExpiryInMinutes = 10080;

    public static DotlessConfiguration GetDefault() => new DotlessConfiguration();

    public static DotlessConfiguration GetDefaultWeb() => new DotlessConfiguration()
    {
      Web = true
    };

    public DotlessConfiguration()
    {
      this.LessSource = typeof (FileReader);
      this.MinifyOutput = false;
      this.Debug = false;
      this.CacheEnabled = true;
      this.HttpExpiryInMinutes = 10080;
      this.Web = false;
      this.SessionMode = DotlessSessionStateMode.Disabled;
      this.SessionQueryParamName = "sstate";
      this.Logger = (Type) null;
      this.LogLevel = LogLevel.Error;
      this.Optimization = 1;
      this.Plugins = new List<IPluginConfigurator>();
      this.MapPathsToWeb = true;
      this.HandleWebCompression = true;
      this.KeepFirstSpecialComment = false;
      this.RootPath = "";
      this.StrictMath = false;
    }

    public DotlessConfiguration(DotlessConfiguration config)
    {
      this.LessSource = config.LessSource;
      this.MinifyOutput = config.MinifyOutput;
      this.Debug = config.Debug;
      this.CacheEnabled = config.CacheEnabled;
      this.Web = config.Web;
      this.SessionMode = config.SessionMode;
      this.SessionQueryParamName = config.SessionQueryParamName;
      this.Logger = (Type) null;
      this.LogLevel = config.LogLevel;
      this.Optimization = config.Optimization;
      this.Plugins = new List<IPluginConfigurator>();
      this.Plugins.AddRange((IEnumerable<IPluginConfigurator>) config.Plugins);
      this.MapPathsToWeb = config.MapPathsToWeb;
      this.DisableUrlRewriting = config.DisableUrlRewriting;
      this.InlineCssFiles = config.InlineCssFiles;
      this.ImportAllFilesAsLess = config.ImportAllFilesAsLess;
      this.HandleWebCompression = config.HandleWebCompression;
      this.DisableParameters = config.DisableParameters;
      this.KeepFirstSpecialComment = config.KeepFirstSpecialComment;
      this.RootPath = config.RootPath;
      this.StrictMath = config.StrictMath;
    }

    public bool KeepFirstSpecialComment { get; set; }

    public bool DisableParameters { get; set; }

    public bool DisableUrlRewriting { get; set; }

    public string RootPath { get; set; }

    [Obsolete("The Variable Redefines feature has been removed to align with less.js")]
    public bool DisableVariableRedefines { get; set; }

    [Obsolete("The Color Compression feature has been removed to align with less.js")]
    public bool DisableColorCompression { get; set; }

    public bool InlineCssFiles { get; set; }

    public bool ImportAllFilesAsLess { get; set; }

    public bool MapPathsToWeb { get; set; }

    public bool MinifyOutput { get; set; }

    public bool Debug { get; set; }

    public bool CacheEnabled { get; set; }

    public int HttpExpiryInMinutes { get; set; }

    public Type LessSource { get; set; }

    public bool Web { get; set; }

    public DotlessSessionStateMode SessionMode { get; set; }

    public string SessionQueryParamName { get; set; }

    public Type Logger { get; set; }

    public LogLevel LogLevel { get; set; }

    public int Optimization { get; set; }

    public bool HandleWebCompression { get; set; }

    public List<IPluginConfigurator> Plugins { get; private set; }

    public bool StrictMath { get; set; }
  }
}
