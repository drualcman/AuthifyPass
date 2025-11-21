namespace AuthifyPass.API.Core.Interfaces.UseCases.RegisterClient;
public interface IRegisterClientController
{
    Task<RegisterClientResponseDto> CreateClientAsync(RegisterClientDto client);
}
