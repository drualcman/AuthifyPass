namespace AuthifyPass.API.Core.Interfaces.UseCases.RegisterUser;
public interface IRegisterUserInputPort
{
    Task<RegisterUserClientResponseDto> RegisterUserAsync(RegisterUserClientDto data, string secredShared);
}
