// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Functions.ColorFunctionBase
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes;
using Nop.Plugin.InstantSearch.Dotless.Parser.Tree;
using Nop.Plugin.InstantSearch.Dotless.Utils;
using System.Linq;

namespace Nop.Plugin.InstantSearch.Dotless.Parser.Functions
{
  public abstract class ColorFunctionBase : Function
  {
    protected override Node Evaluate(Env env)
    {
      Guard.ExpectMinArguments(1, this.Arguments.Count<Node>(), (object) this, this.Location);
      Guard.ExpectNode<Color>(this.Arguments[0], (object) this, this.Arguments[0].Location);
      Color color = this.Arguments[0] as Color;
      if (this.Arguments.Count == 2)
      {
        Guard.ExpectNode<Number>(this.Arguments[1], (object) this, this.Arguments[1].Location);
        Number number = this.Arguments[1] as Number;
        Node node = this.EditColor(color, number);
        if (node != null)
          return node;
      }
      return this.Eval(color);
    }

    protected abstract Node Eval(Color color);

    protected virtual Node EditColor(Color color, Number number) => (Node) null;
  }
}
