namespace Nop.Plugin.InstantSearch.Theme
{
  public interface IThemeService
  {
    Task<bool> CheckActiveThemeAsync(string themeName, string themeVariantsSettingsKey);
  }
}
