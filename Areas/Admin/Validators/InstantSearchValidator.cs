using FluentValidation;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;
using Nop.Plugin.InstantSearch.Areas.Admin.Models;
using System.Linq.Expressions;

namespace Nop.Plugin.InstantSearch.Areas.Admin.Validators
{
  public class InstantSearchValidator : AbstractValidator<InstantSearchSettingsModel>
  {
    public InstantSearchValidator(ILocalizationService localizationService)
    {
      RuleBuilderOptionsExtension.WithMessageAwait<InstantSearchSettingsModel, int>(DefaultValidatorExtensions.InclusiveBetween<InstantSearchSettingsModel, int>((IRuleBuilder<InstantSearchSettingsModel, int>) this.RuleFor<int>((Expression<Func<InstantSearchSettingsModel, int>>) (x => x.NumberOfVisibleProducts)), 1, 1000), localizationService.GetResourceAsync("InstantSearch.Admin.Settings.NumberOfVisibleProducts.Range"));
      RuleBuilderOptionsExtension.WithMessageAwait<InstantSearchSettingsModel, int>(DefaultValidatorExtensions.InclusiveBetween<InstantSearchSettingsModel, int>((IRuleBuilder<InstantSearchSettingsModel, int>) this.RuleFor<int>((Expression<Func<InstantSearchSettingsModel, int>>) (x => x.NumberOfProducts)), 1, 1000), localizationService.GetResourceAsync("InstantSearch.Admin.Settings.NumberOfProducts.Range"));
      RuleBuilderOptionsExtension.WithMessageAwait<InstantSearchSettingsModel, int>(DefaultValidatorExtensions.InclusiveBetween<InstantSearchSettingsModel, int>((IRuleBuilder<InstantSearchSettingsModel, int>) this.RuleFor<int>((Expression<Func<InstantSearchSettingsModel, int>>) (x => x.MinKeywordLength)), 1, 1000), localizationService.GetResourceAsync("InstantSearch.Admin.Settings.MinKeywordLength.Range"));
      RuleBuilderOptionsExtension.WithMessageAwait<InstantSearchSettingsModel, int>(DefaultValidatorExtensions.InclusiveBetween<InstantSearchSettingsModel, int>((IRuleBuilder<InstantSearchSettingsModel, int>) this.RuleFor<int>((Expression<Func<InstantSearchSettingsModel, int>>) (x => x.PictureSize)), 1, 1000), localizationService.GetResourceAsync("InstantSearch.Admin.Settings.PictureSizeInPixels.Range"));
    }
  }
}
