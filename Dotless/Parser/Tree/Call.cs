// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Call
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Parser.Functions;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes;
using Nop.Plugin.InstantSearch.Dotless.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nop.Plugin.InstantSearch.Dotless.Parser.Tree
{
  public class Call : Node
  {
    public string Name { get; set; }

    public NodeList<Node> Arguments { get; set; }

    public Call(string name, NodeList<Node> arguments)
    {
      this.Name = name;
      this.Arguments = arguments;
    }

    protected Call()
    {
    }

    protected override Node CloneCore() => (Node) new Call(this.Name, (NodeList<Node>) this.Arguments.Clone());

    public override Node Evaluate(Env env)
    {
      if (env == null)
        throw new ArgumentNullException(nameof (env));
      IEnumerable<Node> nodes = this.Arguments.Select<Node, Node>((Func<Node, Node>) (a => a.Evaluate(env)));
      Function function = env.GetFunction(this.Name);
      if (function != null)
      {
        function.Name = this.Name;
        function.Location = this.Location;
        return function.Call(env, nodes).ReducedFrom<Node>((Node) this);
      }
      env.Output.Push();
      env.Output.Append(this.Name).Append("(").AppendMany<Node>(nodes, env.Compress ? "," : ", ").Append(")");
      return new TextNode(env.Output.Pop().ToString()).ReducedFrom<Node>((Node) this);
    }

    public override void Accept(IVisitor visitor) => this.Arguments = this.VisitAndReplace<NodeList<Node>>(this.Arguments, visitor);
  }
}
