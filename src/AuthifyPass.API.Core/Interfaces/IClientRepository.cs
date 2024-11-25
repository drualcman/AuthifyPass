using AuthifyPass.API.Core.DTOs;
using AuthifyPass.Entities.DTOs;
using AuthifyPass.Entities.Entities;

namespace AuthifyPass.API.Core.Interfaces;
public interface IClientRepository
{
    Task<Client?> GetByClientIdAsync(string clientId);
    Task AddClientAsync(AddClientDto client);
    Task UpdateClientAsync(UpdateClientDto client);
    Task DeleteClientAsync(string clientId);
}
