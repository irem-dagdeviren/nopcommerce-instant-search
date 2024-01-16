using Nop.Plugin.InstantSearch.Dotless.configuration;
using System;

namespace Nop.Plugin.InstantSearch.Dotless;

public static class Less
{
public static string Parse(string less) => Less.Parse(less, DotlessConfiguration.GetDefault());

public static string Parse(string less, DotlessConfiguration config) => !config.Web ? new EngineFactory(config).GetEngine().TransformToCss(less, (string) null) : throw new Exception("Please use Nop.Plugin.InstantSearch.Dotless.LessWeb.Parse for web applications. This makes sure all web features are available.");
}
