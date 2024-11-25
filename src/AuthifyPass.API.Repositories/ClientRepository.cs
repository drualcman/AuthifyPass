using AuthifyPass.API.Core.DTOs;
using AuthifyPass.API.Core.Interfaces;
using AuthifyPass.API.Repositories.Entities;
using AuthifyPass.API.Repositories.Interfaces;
using AuthifyPass.Entities.DTOs;
using AuthifyPass.Entities.Entities;

namespace AuthifyPass.API.Repositories;
internal class ClientRepository(IWritableDbContext dbWriter, IReadbleDbContext dbReader) : IClientRepository
{
    public async Task AddClientAsync(AddClientDto client)
    {
        DateTime createdAt = DateTime.UtcNow;
        ClientEntity entity = new ClientEntity
        {
            ClientId = client.ClientId,
            SharedSecret = client.SharedSecret,
            Name = client.Name,
            Email = client.Email,
            Password = client.Password,
            CreatedAt = createdAt,
            LastUpdateAt = createdAt,
        };
        await dbWriter.AddClientAsync(entity);
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
