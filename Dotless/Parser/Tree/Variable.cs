// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Variable
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Exceptions;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes;

namespace Nop.Plugin.InstantSearch.Dotless.Parser.Tree
{
  public class Variable : Node
  {
    public string Name { get; set; }

    public Variable(string name) => this.Name = name;

    protected override Node CloneCore() => (Node) new Variable(this.Name);

    public override Node Evaluate(Env env)
    {
      string str = this.Name;
      if (str.StartsWith("@@"))
      {
        Node node = new Variable(str.Substring(1)).Evaluate(env);
        str = "@" + (node is TextNode ? (node as TextNode).Value : node.ToCSS(env));
      }
      Rule rule = !env.IsEvaluatingVariable(str) ? env.FindVariable(str) : throw new ParsingException("Recursive variable definition for " + str, this.Location);
      if ((bool) (Node) rule)
        return rule.Value.Evaluate(env.CreateVariableEvaluationEnv(str));
      throw new ParsingException("variable " + str + " is undefined", this.Location);
    }
  }
}
