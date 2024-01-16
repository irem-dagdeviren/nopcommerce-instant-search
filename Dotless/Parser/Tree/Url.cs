// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Url
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Importers;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes;
using Nop.Plugin.InstantSearch.Dotless.Plugins;
using System.Collections.Generic;

namespace Nop.Plugin.InstantSearch.Dotless.Parser.Tree
{
  public class Url : Node
  {
    public Node Value { get; set; }

    public List<string> ImportPaths { get; set; }

    public IImporter Importer { get; set; }

    public Url(Node value, IImporter importer)
    {
      this.Importer = importer;
      this.ImportPaths = importer.GetCurrentPathsClone();
      this.Value = value;
    }

    public Url(Node value) => this.Value = value;

    public string GetUnadjustedUrl() => this.Value is TextNode textNode ? textNode.Value : (string) null;

    private Node AdjustUrlPath(Node value) => value is TextNode textValue ? (Node) this.AdjustUrlPath(textValue) : value;

    private TextNode AdjustUrlPath(TextNode textValue)
    {
      if (this.Importer != null)
        textValue.Value = this.Importer.AlterUrl(textValue.Value, this.ImportPaths);
      return textValue;
    }

    public override Node Evaluate(Env env) => (Node) new Url(this.AdjustUrlPath(this.Value.Evaluate(env)), this.Importer);

    protected override Node CloneCore() => (Node) new Url(this.Value.Clone(), this.Importer);

    public override void AppendCSS(Env env) => env.Output.Append("url(").Append(this.Value).Append(")");

    public override void Accept(IVisitor visitor) => this.Value = this.VisitAndReplace<Node>(this.Value, visitor);
  }
}
