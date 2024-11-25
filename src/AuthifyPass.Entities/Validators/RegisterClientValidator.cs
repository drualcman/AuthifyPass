using AuthifyPass.Entities.Abstractions;
using AuthifyPass.Entities.DTOs;
using AuthifyPass.Entities.Enums;
using AuthifyPass.Entities.Interfaces;
using AuthifyPass.Entities.Resources;
using Microsoft.Extensions.Localization;

namespace AuthifyPass.Entities.Validators;
internal class RegisterClientValidator : AbstractModelValidator<RegisterClientDto>
{
    public RegisterClientValidator(IValidationService<RegisterClientDto> validationService,
        IStringLocalizer<RegisterClientValidatorErrors> localizer) :
        base(validationService, ValidationConstraint.ValidateIfThereAreNoPreviousErrors)
    {
        AddRuleFor(r => r.Email)
            .NotNull(localizer[nameof(RegisterClientValidatorErrors.Required)])
            .NotEmpty(localizer[nameof(RegisterClientValidatorErrors.EmailEmpty)])
            .EmailAddress(localizer[nameof(RegisterClientValidatorErrors.EmailValidation)]);
        AddRuleFor(r => r.Name)
            .NotNull(localizer[nameof(RegisterClientValidatorErrors.Required)])
            .NotEmpty(localizer[nameof(RegisterClientValidatorErrors.Required)]);
        AddRuleFor(r => r.Password)
            .NotNull(localizer[nameof(RegisterClientValidatorErrors.Required)])
            .NotEmpty(localizer[nameof(RegisterClientValidatorErrors.Required)])
            .Must(ContainsUppercase,localizer[nameof(RegisterClientValidatorErrors.PasswordUppercase)])
            .Must(ContainsLowercase,localizer[nameof(RegisterClientValidatorErrors.PasswordLowercase)])
            .Must(ContainsDigit, localizer[nameof(RegisterClientValidatorErrors.PasswordDigit)])
            .Must(ContainsSpecialCharacter, localizer[nameof(RegisterClientValidatorErrors.PasswordSpecial)])
            .MinimumLength(13, localizer[nameof(RegisterClientValidatorErrors.PasswordLenght)]);
    }

    private bool ContainsUppercase(string input) => ContainsCharacter(input, char.IsUpper);
    private bool ContainsLowercase(string input) => ContainsCharacter(input, char.IsLower);
    private bool ContainsDigit(string input) => ContainsCharacter(input, char.IsDigit);
    private bool ContainsSpecialCharacter(string input) => ContainsCharacter(input, c => !char.IsLetterOrDigit(c));

    private bool ContainsCharacter(string input, Func<char, bool> condition)
    {
        bool result = false;
        if (!string.IsNullOrEmpty(input)) 
        {
            ReadOnlySpan<char> chars = input.AsSpan();
            int i = 0;
            int count = chars.Length;
            do
            {
                result = condition(chars[i]);
                i++;
            } while (i < count && result == false);
        }
        return result;
    }
}
