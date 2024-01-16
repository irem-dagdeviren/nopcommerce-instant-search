// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.D.Core.Parser.Functions.AbsFunction
// Assembly: Nop.Plugin.InstantSearch.D.Core, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.D.Core.dll

using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes;
using Nop.Plugin.InstantSearch.Dotless.Parser.Tree;
using System;

namespace Nop.Plugin.InstantSearch.Dotless.Parser.Functions
{
  public class AbsFunction : NumberFunctionBase
  {
    protected override Node Eval(Env env, Number number, Node[] args)
    {
      this.WarnNotSupportedByLessJS("abs(number)");
      return (Node) new Number(Math.Abs(number.Value), number.Unit);
    }
  }
}
