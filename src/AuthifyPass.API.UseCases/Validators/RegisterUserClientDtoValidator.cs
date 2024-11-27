using AuthifyPass.Entities.Abstractions;
using AuthifyPass.Entities.Enums;

namespace AuthifyPass.API.UseCases.Validators;
internal class RegisterUserClientDtoValidator : AbstractModelValidator<RegisterUserClientDto>
{
    public RegisterUserClientDtoValidator(IValidationService<RegisterUserClientDto> validationService,
        IStringLocalizer<ValidateUserCodeContent> localizer) :
        base(validationService, ValidationConstraint.ValidateIfThereAreNoPreviousErrors)
    {
        AddRuleFor(r => r.UserId)
            .NotNull(localizer[nameof(ValidateUserCodeContent.Required)])
            .NotEmpty(localizer[nameof(ValidateUserCodeContent.Required)]);
        AddRuleFor(r => r.ClientId)
            .NotNull(localizer[nameof(ValidateUserCodeContent.Required)])
            .NotEmpty(localizer[nameof(ValidateUserCodeContent.Required)]);
    }
}
