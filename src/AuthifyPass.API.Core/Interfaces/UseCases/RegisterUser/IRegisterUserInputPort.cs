namespace AuthifyPass.API.Core.Interfaces.UseCases.RegisterUser;
public interface IRegisterUserInputPort
{
    Task<string> RegisterUserAsync(RegisterUserClientDto data, string secredShared);
}
