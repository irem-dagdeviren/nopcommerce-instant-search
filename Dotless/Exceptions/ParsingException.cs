// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Exceptions.ParsingException
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Parser;
using System;

namespace Nop.Plugin.InstantSearch.Dotless.Exceptions
{
  public class ParsingException : Exception
  {
    public NodeLocation Location { get; set; }

    public NodeLocation CallLocation { get; set; }

    public ParsingException(string message, NodeLocation location)
      : this(message, (Exception) null, location, (NodeLocation) null)
    {
    }

    public ParsingException(string message, NodeLocation location, NodeLocation callLocation)
      : this(message, (Exception) null, location, callLocation)
    {
    }

    public ParsingException(Exception innerException, NodeLocation location)
      : this(innerException, location, (NodeLocation) null)
    {
    }

    public ParsingException(
      Exception innerException,
      NodeLocation location,
      NodeLocation callLocation)
      : this(innerException.Message, innerException, location, callLocation)
    {
    }

    public ParsingException(string message, Exception innerException, NodeLocation location)
      : this(message, innerException, location, (NodeLocation) null)
    {
    }

    public ParsingException(
      string message,
      Exception innerException,
      NodeLocation location,
      NodeLocation callLocation)
      : base(message, innerException)
    {
      this.Location = location;
      this.CallLocation = callLocation;
    }
  }
}
