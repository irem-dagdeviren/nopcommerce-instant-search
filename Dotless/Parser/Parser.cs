// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Parser
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.configuration;
using Nop.Plugin.InstantSearch.Dotless.Exceptions;
using Nop.Plugin.InstantSearch.Dotless.Importers;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure;
using Nop.Plugin.InstantSearch.Dotless.Parser.Tree;
using Nop.Plugin.InstantSearch.Dotless.Stylizers;
using System;

namespace Nop.Plugin.InstantSearch.Dotless.Parser
{
  public class Parser
  {
    private INodeProvider _nodeProvider;
    private IImporter _importer;
    private const int defaultOptimization = 1;
    private const bool defaultDebug = false;

    public Tokenizer Tokenizer { get; set; }

    public IStylizer Stylizer { get; set; }

    public string FileName { get; set; }

    public bool Debug { get; set; }

    public string CurrentDirectory
    {
      get => this.Importer.CurrentDirectory;
      set => this.Importer.CurrentDirectory = value;
    }

    public INodeProvider NodeProvider
    {
      get => this._nodeProvider ?? (this._nodeProvider = (INodeProvider) new DefaultNodeProvider());
      set => this._nodeProvider = value;
    }

    public IImporter Importer
    {
      get => this._importer;
      set
      {
        this._importer = value;
        this._importer.Parser = (Func<Nop.Plugin.InstantSearch.Dotless.Parser.Parser>) (() => new Nop.Plugin.InstantSearch.Dotless.Parser.Parser(this.Tokenizer.Optimization, this.Stylizer, this._importer)
        {
          NodeProvider = this.NodeProvider,
          Debug = this.Debug,
          CurrentDirectory = this.CurrentDirectory,
          StrictMath = this.StrictMath
        });
      }
    }

    public bool StrictMath { get; set; }

    public Parser()
      : this(1, false)
    {
    }

    public Parser(bool debug)
      : this(1, debug)
    {
    }

    public Parser(int optimization)
      : this(optimization, (IStylizer) new PlainStylizer(), (IImporter) new Nop.Plugin.InstantSearch.Dotless.Importers.Importer(), false)
    {
    }

    public Parser(int optimization, bool debug)
      : this(optimization, (IStylizer) new PlainStylizer(), (IImporter) new Nop.Plugin.InstantSearch.Dotless.Importers.Importer(), debug)
    {
    }

    public Parser(IStylizer stylizer, IImporter importer)
      : this(1, stylizer, importer, false)
    {
    }

    public Parser(IStylizer stylizer, IImporter importer, bool debug)
      : this(1, stylizer, importer, debug)
    {
    }

    public Parser(int optimization, IStylizer stylizer, IImporter importer)
      : this(optimization, stylizer, importer, false)
    {
    }

    public Parser(int optimization, IStylizer stylizer, IImporter importer, bool debug)
    {
      this.Stylizer = stylizer;
      this.Importer = importer;
      this.Debug = debug;
      this.Tokenizer = new Tokenizer(optimization);
    }

    public Parser(DotlessConfiguration config, IStylizer stylizer, IImporter importer)
      : this(config.Optimization, stylizer, importer, config.Debug)
    {
    }

    public Ruleset Parse(string input, string fileName)
    {
      this.FileName = fileName;
      Ruleset ruleset;
      try
      {
        this.Tokenizer.SetupInput(input, fileName);
        ruleset = (Ruleset) new Root(new Parsers(this.NodeProvider).Primary(this), new Func<ParsingException, ParserException>(this.GenerateParserError));
      }
      catch (ParsingException ex)
      {
        throw this.GenerateParserError(ex);
      }
      if (!this.Tokenizer.HasCompletedParsing())
        throw this.GenerateParserError(new ParsingException("Content after finishing parsing (missing opening bracket?)", this.Tokenizer.GetNodeLocation(this.Tokenizer.Location.Index)));
      return ruleset;
    }

    private ParserException GenerateParserError(ParsingException parsingException)
    {
      NodeLocation location = parsingException.Location;
      string message = parsingException.Message;
      NodeLocation callLocation = parsingException.CallLocation;
      string error = message;
      Zone callZone = callLocation != null ? new Zone(callLocation) : (Zone) null;
      Zone zone = new Zone(location, error, callZone);
      return new ParserException(this.Stylizer.Stylize(zone), (Exception) parsingException, zone);
    }
  }
}
