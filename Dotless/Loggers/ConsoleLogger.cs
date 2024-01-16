// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Loggers.ConsoleLogger
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\
//
// \Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.configuration;
using System;

namespace Nop.Plugin.InstantSearch.Dotless.Loggers
{
  public class ConsoleLogger : Logger
  {
    public ConsoleLogger(LogLevel level)
      : base(level)
    {
    }

    public ConsoleLogger(DotlessConfiguration config)
      : this(config.LogLevel)
    {
    }

    protected override void Log(string message) => Console.WriteLine(message);
  }
}
