﻿// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Functions.ExtractFunction
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes;
using Nop.Plugin.InstantSearch.Dotless.Parser.Tree;
using Nop.Plugin.InstantSearch.Dotless.Utils;

namespace Nop.Plugin.InstantSearch.Dotless.Parser.Functions
{
  public class ExtractFunction : ListFunctionBase
  {
    protected override Node Eval(Env env, Node[] list, Node[] args)
    {
      Guard.ExpectNumArguments(1, args.Length, (object) this, this.Location);
      Guard.ExpectNode<Number>(args[0], (object) this, args[0].Location);
      int num = (int) (args[0] as Number).Value;
      return list[num - 1];
    }
  }
}
