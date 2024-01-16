// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.ParameterDecorator
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Exceptions;
using Nop.Plugin.InstantSearch.Dotless.Parameters;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nop.Plugin.InstantSearch.Dotless
{
  public class ParameterDecorator : ILessEngine
  {
    public readonly ILessEngine Underlying;
    private readonly IParameterSource parameterSource;

    public ParameterDecorator(ILessEngine underlying, IParameterSource parameterSource)
    {
      this.Underlying = underlying;
      this.parameterSource = parameterSource;
    }

    public string TransformToCss(string source, string fileName)
    {
      StringBuilder stringBuilder = new StringBuilder();
      IEnumerable<KeyValuePair<string, string>> keyValuePairs = this.parameterSource.GetParameters().Where<KeyValuePair<string, string>>(new Func<KeyValuePair<string, string>, bool>(ParameterDecorator.ValueIsNotNullOrEmpty));
      Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser = new Nop.Plugin.InstantSearch.Dotless.Parser.Parser();
      stringBuilder.Append(source);
      foreach (KeyValuePair<string, string> keyValuePair in keyValuePairs)
      {
        stringBuilder.AppendLine();
        string input = string.Format("@{0}: {1};", (object) keyValuePair.Key, (object) keyValuePair.Value);
        try
        {
          parser.Parse(input, "").ToCSS(new Env());
          stringBuilder.Append(input);
        }
        catch (ParserException ex)
        {
          stringBuilder.AppendFormat("/* Omitting variable '{0}'. The expression '{1}' is not valid. */", (object) keyValuePair.Key, (object) keyValuePair.Value);
        }
      }
      return this.Underlying.TransformToCss(stringBuilder.ToString(), fileName);
    }

    public IEnumerable<string> GetImports() => this.Underlying.GetImports();

    public void ResetImports() => this.Underlying.ResetImports();

    public bool LastTransformationSuccessful => this.Underlying.LastTransformationSuccessful;

    private static bool ValueIsNotNullOrEmpty(KeyValuePair<string, string> kvp) => !string.IsNullOrEmpty(kvp.Value);

    public string CurrentDirectory
    {
      get => this.Underlying.CurrentDirectory;
      set => this.Underlying.CurrentDirectory = value;
    }
  }
}
