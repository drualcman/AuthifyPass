namespace AuthifyPass.Views.Validators;
internal class TwoFactorCodeValidator : AbstractModelValidator<TwoFactorCode>
{
    public TwoFactorCodeValidator(IValidationService<TwoFactorCode> validationService,
        IStringLocalizer<ValidateTwoFactorCodeModelContent> localizer)
        : base(validationService)
    {
        AddRuleFor(r => r.Description)
            .NotEmpty(localizer[nameof(ValidateTwoFactorCodeModelContent.DescriptionError)]);
        AddRuleFor(r => r.ClientId)
            .NotEmpty(localizer[nameof(ValidateTwoFactorCodeModelContent.ClientIdError)]);
        AddRuleFor(r => r.SharedKey)
            .NotEmpty(localizer[nameof(ValidateTwoFactorCodeModelContent.ShareKeyError)]);
        AddRuleFor(r => r.Name)
            .NotEmpty(localizer[nameof(ValidateTwoFactorCodeModelContent.NameError)]);
    }
}
