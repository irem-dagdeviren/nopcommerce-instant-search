using Nop.Core.Caching;

namespace Nop.Plugin.InstantSearch.Infrastructure.Constants
{
  public static class CacheKeys
  {
    public const string NOP_INSTANT_SEARCH_PATTERN_KEY = "nop.pres.nop.instant.search";

    public static CacheKey NOP_INSTANT_SEARCH_CATEGORIES_MODEL_KEY => new CacheKey("nop.pres.nop.instant.search-categories-{0}-{1}-{2}", new string[1]
    {
      "nop.pres.nop.instant.search"
    });

    public static CacheKey NOP_INSTANT_SEARCH_MANUFACTURERS_MODEL_KEY => new CacheKey("nop.pres.nop.instant.search-manufacturers-{0}-{1}-{2}", new string[1]
    {
      "nop.pres.nop.instant.search"
    });

    public static CacheKey NOP_INSTANT_SEARCH_VENDORS_MODEL_KEY => new CacheKey("nop.pres.nop.instant.search-vendors-{0}-{1}-{2}", new string[1]
    {
      "nop.pres.nop.instant.search"
    });
  }
}
