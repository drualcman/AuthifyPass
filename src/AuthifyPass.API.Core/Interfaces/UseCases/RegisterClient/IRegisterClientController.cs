namespace AuthifyPass.API.Core.Interfaces.UseCases.RegisterClient;
public interface IRegisterClientController
{
    Task<RegisterClientResultDto> CreateClientAsync(RegisterClientDto client);
}
