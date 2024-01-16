// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Loggers.Logger
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

namespace Nop.Plugin.InstantSearch.Dotless.Loggers
{
  public abstract class Logger : ILogger
  {
    public LogLevel Level { get; set; }

    protected Logger(LogLevel level) => this.Level = level;

    public void Log(LogLevel level, string message)
    {
      if (this.Level > level)
        return;
      this.Log(message);
    }

    protected abstract void Log(string message);

    public void Info(string message) => this.Log(LogLevel.Info, message);

    public void Debug(string message) => this.Log(LogLevel.Debug, message);

    public void Warn(string message) => this.Log(LogLevel.Warn, message);

    public void Error(string message) => this.Log(LogLevel.Error, message);

    public void Info(string message, params object[] args) => this.Log(LogLevel.Info, string.Format(message, args));

    public void Debug(string message, params object[] args) => this.Log(LogLevel.Debug, string.Format(message, args));

    public void Warn(string message, params object[] args) => this.Log(LogLevel.Warn, string.Format(message, args));

    public void Error(string message, params object[] args) => this.Log(LogLevel.Error, string.Format(message, args));
  }
}
