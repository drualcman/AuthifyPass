using AuthifyPass.API.Core.DTOs;
using AuthifyPass.Entities.DTOs;

namespace AuthifyPass.API.Core.Interfaces.UseCases.RegisterClient;
public interface IRegisterClientController
{
    Task<RegisterClientResultDto> CreateClientAsync(RegisterClientDto client);
}
