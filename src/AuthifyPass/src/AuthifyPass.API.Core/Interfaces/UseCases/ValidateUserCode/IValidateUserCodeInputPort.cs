namespace AuthifyPass.API.Core.Interfaces.UseCases.ValidateUserCode;
public interface IValidateUserCodeInputPort
{
    Task<bool> ValidateUserCode(ValidateUserCodeDto data, string secretShared);
}
