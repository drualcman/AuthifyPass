namespace AuthifyPass.Views.Validators;
internal class TwoFactorCodeModelValidator(IModelValidatorHub<TwoFactorCode> validator)
    : AbstractViewModelValidator<TwoFactorCode, IAdd2FAViewModel<TwoFactorCode>>(validator, ValidationConstraint.AlwaysValidate)
{
}
