// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Functions.ListFunctionBase
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Exceptions;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes;
using Nop.Plugin.InstantSearch.Dotless.Parser.Tree;
using Nop.Plugin.InstantSearch.Dotless.Utils;
using System.Linq;

namespace Nop.Plugin.InstantSearch.Dotless.Parser.Functions
{
  public abstract class ListFunctionBase : Function
  {
    protected override Node Evaluate(Env env)
    {
      Guard.ExpectMinArguments(1, this.Arguments.Count, (object) this, this.Location);
      Guard.ExpectNodeToBeOneOf<Expression, Value>(this.Arguments[0], (object) this, this.Arguments[0].Location);
      if (this.Arguments[0] is Expression)
      {
        Expression expression = this.Arguments[0] as Expression;
        Node[] array = this.Arguments.Skip<Node>(1).ToArray<Node>();
        return this.Eval(env, expression.Value.ToArray<Node>(), array);
      }
      Value obj = this.Arguments[0] is Value ? this.Arguments[0] as Value : throw new ParsingException(string.Format("First argument to the list function was a {0}", (object) this.Arguments[0].GetType().Name.ToLowerInvariant()), this.Location);
      Node[] array1 = this.Arguments.Skip<Node>(1).ToArray<Node>();
      return this.Eval(env, obj.Values.ToArray<Node>(), array1);
    }

    protected abstract Node Eval(Env env, Node[] list, Node[] args);
  }
}
