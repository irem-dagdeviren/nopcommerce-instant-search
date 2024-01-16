// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Exceptions.ParserException
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Parser;
using System;

namespace Nop.Plugin.InstantSearch.Dotless.Exceptions
{
  public class ParserException : Exception
  {
    public ParserException(string message)
      : base(message)
    {
    }

    public ParserException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    public ParserException(string message, Exception innerException, Zone errorLocation)
      : base(message, innerException)
    {
      this.ErrorLocation = errorLocation;
    }

    public Zone ErrorLocation { get; set; }
  }
}
