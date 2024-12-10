using AuthifyPass.Entities.Abstractions;
using AuthifyPass.Views.Resources;
using Microsoft.Extensions.Localization;

namespace AuthifyPass.Views.Validators;
internal class TwoFactorCodeValidator : AbstractModelValidator<TwoFactorCode>
{
    public TwoFactorCodeValidator(IValidationService<TwoFactorCode> validationService,
        IStringLocalizer<ValidateTwoFactorCodeModelContent> localizer)
        : base(validationService)
    {
        AddRuleFor(r => r.Title)
            .NotEmpty(localizer[nameof(ValidateTwoFactorCodeModelContent.TitleError)]);
        AddRuleFor(r => r.ClientId)
            .NotEmpty(localizer[nameof(ValidateTwoFactorCodeModelContent.ClientIdError)]);
        AddRuleFor(r => r.SharedKey)
            .NotEmpty(localizer[nameof(ValidateTwoFactorCodeModelContent.ShareKeyError)]);
        AddRuleFor(r => r.Name)
            .NotEmpty(localizer[nameof(ValidateTwoFactorCodeModelContent.NameError)]);
    }
}
