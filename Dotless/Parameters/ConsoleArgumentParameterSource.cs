// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parameters.ConsoleArgumentParameterSource
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using System.Collections.Generic;

namespace Nop.Plugin.InstantSearch.Dotless.Parameters
{
  public class ConsoleArgumentParameterSource : IParameterSource
  {
    public static IDictionary<string, string> ConsoleArguments = (IDictionary<string, string>) new Dictionary<string, string>();

    public IDictionary<string, string> GetParameters() => ConsoleArgumentParameterSource.ConsoleArguments;
  }
}
