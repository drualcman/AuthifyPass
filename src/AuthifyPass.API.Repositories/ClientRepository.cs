using AuthifyPass.API.Core.DTOs;
using AuthifyPass.API.Core.Interfaces;
using AuthifyPass.Entities.DTOs;
using AuthifyPass.Entities.Entities;

namespace AuthifyPass.API.Repositories;
internal class ClientRepository : IClientRepository
{
    public Task AddClientAsync(AddClientDto client)
    {
        return Task.CompletedTask;
    }

    public Task DeleteClientAsync(string clientId)
    {
        return Task.CompletedTask;
    }

    public Task<Client?> GetByClientIdAsync(string clientId)
    {
        return Task.FromResult(new Client(clientId,"","","","",DateTime.UtcNow));
    }

    public Task UpdateClientAsync(UpdateClientDto client)
    {
        return Task.CompletedTask;
    }
}
