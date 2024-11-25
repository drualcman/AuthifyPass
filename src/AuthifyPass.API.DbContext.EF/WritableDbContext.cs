using AuthifyPass.API.Core.Interfaces;
using AuthifyPass.API.Repositories.Entities;

namespace AuthifyPass.API.DbContext.EF;
internal class WritableDbContext : IWritableDbContext
{
    public Task AddClientAsync(ClientEntity client)
    {
        return Task.CompletedTask;
    }

    public Task AddUserSecretAsync(UserSecretEntity userSecret)
    {
        return Task.CompletedTask;
    }

    public Task DeleteClientAsync(string clientId)
    {
        return Task.CompletedTask;
    }

    public Task UpdateClientAsync(ClientEntity client)
    {
        return Task.CompletedTask;
    }

    public Task UpdateUserSecretAsync(UserSecretEntity userSecret)
    {
        return Task.CompletedTask;
    }
}
