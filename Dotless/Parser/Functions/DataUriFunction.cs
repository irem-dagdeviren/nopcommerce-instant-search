// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Functions.DataUriFunction
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Exceptions;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes;
using Nop.Plugin.InstantSearch.Dotless.Parser.Tree;
using Nop.Plugin.InstantSearch.Dotless.Utils;
using System;
using System.IO;

namespace Nop.Plugin.InstantSearch.Dotless.Parser.Functions
{
  public class DataUriFunction : Function
  {
    protected override Node Evaluate(Env env)
    {
      string dataUriFilename = this.GetDataUriFilename();
      string base64 = this.ConvertFileToBase64(dataUriFilename);
      return (Node) new TextNode(string.Format("url(\"data:{0};base64,{1}\")", (object) this.GetMimeType(dataUriFilename), (object) base64));
    }

    private string GetDataUriFilename()
    {
      Guard.ExpectMinArguments(1, this.Arguments.Count, (object) this, this.Location);
      Node actual = this.Arguments[0];
      if (this.Arguments.Count > 1)
        actual = this.Arguments[1];
      Guard.ExpectNode<Quoted>(actual, (object) this, this.Location);
      string dataUriFilename = ((TextNode) actual).Value;
      Guard.Expect(!dataUriFilename.StartsWith("http://") && !dataUriFilename.StartsWith("https://"), string.Format("Invalid filename passed to data-uri '{0}'. Filename must be a local file", (object) dataUriFilename), this.Location);
      return dataUriFilename;
    }

    private string ConvertFileToBase64(string filename)
    {
      try
      {
        return Convert.ToBase64String(File.ReadAllBytes(filename));
      }
      catch (IOException ex)
      {
        throw new ParsingException(string.Format("Data-uri function could not read file '{0}'", (object) filename), (Exception) ex, this.Location);
      }
    }

    private string GetMimeType(string filename)
    {
      if (this.Arguments.Count <= 1)
        return new MimeTypeLookup().ByFilename(filename);
      Guard.ExpectNode<Quoted>(this.Arguments[0], (object) this, this.Location);
      string mimeType = ((TextNode) this.Arguments[0]).Value;
      if (mimeType.IndexOf(';') > -1)
        mimeType = mimeType.Split(';')[0];
      return mimeType;
    }
  }
}
