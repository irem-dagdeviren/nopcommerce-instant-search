// Decompiled with JetBrains decompiler
// Type:
// .Parser.Tree.Import
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Exceptions;
using Nop.Plugin.InstantSearch.Dotless.Importers;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes;
using Nop.Plugin.InstantSearch.Dotless.Plugins;
using Nop.Plugin.InstantSearch.Dotless.Utils;

namespace Nop.Plugin.InstantSearch.Dotless.Parser.Tree
{
  public class Import : Directive
  {
    private readonly ReferenceVisitor referenceVisitor = new ReferenceVisitor(true);
    private ImportAction? _importAction;

    protected Node OriginalPath { get; set; }

    public string Path { get; set; }

    public Ruleset InnerRoot { get; set; }

    public string InnerContent { get; set; }

    public Node Features { get; set; }

    public ImportOptions ImportOptions { get; set; }

    public Import(Quoted path, Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Value features, ImportOptions option)
      : this((Node) path, features, option)
    {
      this.OriginalPath = (Node) path;
    }

    public Import(Url path, Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Value features, ImportOptions option)
      : this((Node) path, features, option)
    {
      this.OriginalPath = (Node) path;
      this.Path = path.GetUnadjustedUrl();
    }

    private Import(Node originalPath, Node features)
    {
      this.OriginalPath = originalPath;
      this.Features = features;
      this._importAction = new ImportAction?(ImportAction.LeaveImport);
    }

    private Import(Node path, Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Value features, ImportOptions option)
    {
      this.OriginalPath = path != null ? path : throw new ParserException("Imports do not allow expressions");
      this.Features = (Node) features;
      this.ImportOptions = option;
    }

    private ImportAction GetImportAction(IImporter importer)
    {
      if (!this._importAction.HasValue)
        this._importAction = new ImportAction?(importer.Import(this));
      return this._importAction.Value;
    }

    public override void AppendCSS(Env env, Context context)
    {
      switch (this.GetImportAction(env.Parser.Importer))
      {
        case ImportAction.ImportCss:
          env.Output.Append(this.InnerContent);
          break;
        case ImportAction.ImportNothing:
          break;
        default:
          env.Output.Append("@import ").Append(this.OriginalPath.ToCSS(env));
          if ((bool) this.Features)
            env.Output.Append(" ").Append(this.Features);
          env.Output.Append(";");
          if (env.Compress)
            break;
          env.Output.Append("\n");
          break;
      }
    }

    public override void Accept(IVisitor visitor)
    {
      this.Features = this.VisitAndReplace<Node>(this.Features, visitor, true);
      ImportAction? importAction1 = this._importAction;
      ImportAction importAction2 = ImportAction.ImportLess;
      if ((importAction1.GetValueOrDefault() == importAction2 ? (importAction1.HasValue ? 1 : 0) : 0) == 0)
        return;
      this.InnerRoot = this.VisitAndReplace<Ruleset>(this.InnerRoot, visitor);
    }

    public override Node Evaluate(Env env)
    {
      this.OriginalPath = this.OriginalPath.Evaluate(env);
      if (this.OriginalPath is Quoted originalPath)
        this.Path = originalPath.Value;
      ImportAction importAction = this.GetImportAction(env.Parser.Importer);
      if (importAction == ImportAction.ImportNothing)
        return (Node) new NodeList().ReducedFrom<NodeList>((Node) this);
      Node features1 = (Node) null;
      if ((bool) this.Features)
        features1 = this.Features.Evaluate(env);
      switch (importAction)
      {
        case ImportAction.ImportCss:
          Import import = new Import(this.OriginalPath, (Node) null)
          {
            _importAction = new ImportAction?(ImportAction.ImportCss),
            InnerContent = this.InnerContent
          };
          if (!(bool) features1)
            return (Node) import;
          Node features2 = features1;
          NodeList rules1 = new NodeList();
          rules1.Add((Node) import);
          return (Node) new Media(features2, rules1);
        case ImportAction.LeaveImport:
          return (Node) new Import(this.OriginalPath, features1);
        default:
          using (env.Parser.Importer.BeginScope(this))
          {
            if (this.IsReference || this.IsOptionSet(this.ImportOptions, ImportOptions.Reference))
            {
              this.IsReference = true;
              this.Accept((IVisitor) this.referenceVisitor);
            }
            NodeHelper.RecursiveExpandNodes<Import>(env, this.InnerRoot);
          }
          NodeList rules2 = new NodeList(this.InnerRoot.Rules).ReducedFrom<NodeList>((Node) this);
          return (bool) features1 ? (Node) new Media(features1, rules2) : (Node) rules2;
      }
    }

    private bool IsOptionSet(ImportOptions options, ImportOptions test) => (options & test) == test;
  }
}
