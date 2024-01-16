// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Loggers.ILogger
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

namespace Nop.Plugin.InstantSearch.Dotless.Loggers
{
  public interface ILogger
  {
    void Log(LogLevel level, string message);

    void Info(string message);

    void Info(string message, params object[] args);

    void Debug(string message);

    void Debug(string message, params object[] args);

    void Warn(string message);

    void Warn(string message, params object[] args);

    void Error(string message);

    void Error(string message, params object[] args);
  }
}
