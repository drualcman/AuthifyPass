using AuthifyPass.Entities.Abstractions;
using AuthifyPass.Entities.Enums;
using AuthifyPass.Views.ViewModel;

namespace AuthifyPass.Views.Validators;
internal class TwoFactorCodeModelValidator(IModelValidatorHub<TwoFactorCode> validator)
    : AbstractViewModelValidator<TwoFactorCode, Add2FAViewModel>(validator, ValidationConstraint.AlwaysValidate)
{
}
