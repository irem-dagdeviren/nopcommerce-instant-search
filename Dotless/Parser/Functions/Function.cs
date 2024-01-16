// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Functions.Function
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Loggers;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes;
using System.Collections.Generic;
using System.Linq;

namespace Nop.Plugin.InstantSearch.Dotless.Parser.Functions
{
  public abstract class Function
  {
    public string Name { get; set; }

    protected List<Node> Arguments { get; set; }

    public ILogger Logger { get; set; }

    public NodeLocation Location { get; set; }

    public Node Call(Env env, IEnumerable<Node> arguments)
    {
      this.Arguments = arguments.ToList<Node>();
      Node node = this.Evaluate(env);
      node.Location = this.Location;
      return node;
    }

    protected abstract Node Evaluate(Env env);

    public override string ToString() => string.Format("function '{0}'", (object) this.Name.ToLowerInvariant());

    protected void WarnNotSupportedByLessJS(string functionPattern) => this.WarnNotSupportedByLessJS(functionPattern, (string) null, (string) null);

    protected void WarnNotSupportedByLessJS(string functionPattern, string replacementPattern) => this.WarnNotSupportedByLessJS(functionPattern, replacementPattern, (string) null);

    protected void WarnNotSupportedByLessJS(
      string functionPattern,
      string replacementPattern,
      string extraInfo)
    {
      if (string.IsNullOrEmpty(replacementPattern))
        this.Logger.Info("{0} is not supported by less.js, so this will work but not compile with other less implementations.{1}", (object) functionPattern, (object) extraInfo);
      else
        this.Logger.Warn("{0} is not supported by less.js, so this will work but not compile with other less implementations. You may want to consider using {1} which does the same thing and is supported.{2}", (object) functionPattern, (object) replacementPattern, (object) extraInfo);
    }
  }
}
