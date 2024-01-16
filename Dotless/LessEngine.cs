// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.LessEngine
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.configuration;
using Nop.Plugin.InstantSearch.Dotless.Exceptions;
using Nop.Plugin.InstantSearch.Dotless.Loggers;
using Nop.Plugin.InstantSearch.Dotless.Parser;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure;
using Nop.Plugin.InstantSearch.Dotless.Parser.Tree;
using Nop.Plugin.InstantSearch.Dotless.Plugins;
using Nop.Plugin.InstantSearch.Dotless.Stylizers;
using System;
using System.Collections.Generic;

namespace Nop.Plugin.InstantSearch.Dotless
{
  public class LessEngine : ILessEngine
  {
    public Nop.Plugin.InstantSearch.Dotless.Parser.Parser Parser { get; set; }

    public ILogger Logger { get; set; }

    public bool Compress { get; set; }

    public bool Debug { get; set; }

    [Obsolete("The Variable Redefines feature has been removed to align with less.js")]
    public bool DisableVariableRedefines { get; set; }

    [Obsolete("The Color Compression feature has been removed to align with less.js")]
    public bool DisableColorCompression { get; set; }

    public bool KeepFirstSpecialComment { get; set; }

    public bool StrictMath { get; set; }

    public Env Env { get; set; }

    public IEnumerable<IPluginConfigurator> Plugins { get; set; }

    public bool LastTransformationSuccessful { get; private set; }

    public string CurrentDirectory
    {
      get => this.Parser.CurrentDirectory;
      set => this.Parser.CurrentDirectory = value;
    }

    public LessEngine(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser, ILogger logger, DotlessConfiguration config)
      : this(parser, logger, config.MinifyOutput, config.Debug, config.DisableVariableRedefines, config.DisableColorCompression, config.KeepFirstSpecialComment, (IEnumerable<IPluginConfigurator>) config.Plugins)
    {
    }

    public LessEngine(
      Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser,
      ILogger logger,
      bool compress,
      bool debug,
      bool disableVariableRedefines,
      bool disableColorCompression,
      bool keepFirstSpecialComment,
      bool strictMath,
      IEnumerable<IPluginConfigurator> plugins)
    {
      this.Parser = parser;
      this.Logger = logger;
      this.Compress = compress;
      this.Debug = debug;
      this.Plugins = plugins;
      this.KeepFirstSpecialComment = keepFirstSpecialComment;
      this.StrictMath = strictMath;
    }

    public LessEngine(
      Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser,
      ILogger logger,
      bool compress,
      bool debug,
      bool disableVariableRedefines,
      bool disableColorCompression,
      bool keepFirstSpecialComment,
      IEnumerable<IPluginConfigurator> plugins)
      : this(parser, logger, compress, debug, disableVariableRedefines, disableColorCompression, keepFirstSpecialComment, false, plugins)
    {
    }

    public LessEngine(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser, ILogger logger, bool compress, bool debug)
      : this(parser, logger, compress, debug, false, false, false, (IEnumerable<IPluginConfigurator>) null)
    {
    }

    public LessEngine(
      Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser,
      ILogger logger,
      bool compress,
      bool debug,
      bool disableVariableRedefines)
      : this(parser, logger, compress, debug, disableVariableRedefines, false, false, (IEnumerable<IPluginConfigurator>) null)
    {
    }

    public LessEngine(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser)
      : this(parser, (ILogger) new ConsoleLogger(LogLevel.Error), false, false, false, false, false, (IEnumerable<IPluginConfigurator>) null)
    {
    }

    public LessEngine()
      : this(new Nop.Plugin.InstantSearch.Dotless.Parser.Parser())
    {
    }

    public string TransformToCss(string source, string fileName)
    {
      try
      {
        this.Parser.StrictMath = this.StrictMath;
        Ruleset ruleset = this.Parser.Parse(source, fileName);
        Env env1 = this.Env;
        if (env1 == null)
          env1 = new Env(this.Parser)
          {
            Compress = this.Compress,
            Debug = this.Debug,
            KeepFirstSpecialComment = this.KeepFirstSpecialComment
          };
        Env env = env1;
        if (this.Plugins != null)
        {
          foreach (IPluginConfigurator plugin in this.Plugins)
            env.AddPlugin(plugin.CreatePlugin());
        }
        string css = ruleset.ToCSS(env);
        PlainStylizer stylizer = new PlainStylizer();
        foreach (Extender unmatchedExtension in env.FindUnmatchedExtensions())
          this.Logger.Warn("Warning: extend '{0}' has no matches {1}\n", (object) unmatchedExtension.BaseSelector.ToCSS(env).Trim(), (object) stylizer.Stylize(new Zone(unmatchedExtension.Extend.Location)).Trim());
        ruleset.Accept(DelegateVisitor.For<Media>((Action<Media>) (m =>
        {
          foreach (Extender unmatchedExtension in m.FindUnmatchedExtensions())
            this.Logger.Warn("Warning: extend '{0}' has no matches {1}\n", (object) unmatchedExtension.BaseSelector.ToCSS(env).Trim(), (object) stylizer.Stylize(new Zone(unmatchedExtension.Extend.Location)).Trim());
        })));
        this.LastTransformationSuccessful = true;
        return css;
      }
      catch (ParserException ex)
      {
        this.LastTransformationSuccessful = false;
        this.LastTransformationError = ex;
        this.Logger.Error(ex.Message);
      }
      return "";
    }

    public ParserException LastTransformationError { get; set; }

    public IEnumerable<string> GetImports() => this.Parser.Importer.GetImports();

    public void ResetImports() => this.Parser.Importer.ResetImports();
  }
}
