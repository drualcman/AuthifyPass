using AuthifyPass.Entities.DTOs;

namespace AuthifyPass.API.Core.Interfaces;
public interface IRegisterClientInputPort
{
    Task CreateClientAsync(RegisterClientDto client);
}
