namespace AuthifyPass.API.Core.Validators;
internal class ValidateUserCodeDtoValidator : AbstractModelValidator<ValidateUserCodeDto>
{
    public ValidateUserCodeDtoValidator(IValidationService<ValidateUserCodeDto> validationService,
        IStringLocalizer<ValidateUserCodeContent> localizer) :
        base(validationService, ValidationConstraint.ValidateIfThereAreNoPreviousErrors)
    {
        AddRuleFor(r => r.UserId)
            .NotNull(localizer[nameof(ValidateUserCodeContent.Required)])
            .NotEmpty(localizer[nameof(ValidateUserCodeContent.Required)]);
        AddRuleFor(r => r.ClientId)
            .NotNull(localizer[nameof(ValidateUserCodeContent.Required)])
            .NotEmpty(localizer[nameof(ValidateUserCodeContent.Required)]);
        AddRuleFor(r => r.UserCode)
            .NotNull(localizer[nameof(ValidateUserCodeContent.Required)])
            .NotEmpty(localizer[nameof(ValidateUserCodeContent.Required)]);
    }
}
