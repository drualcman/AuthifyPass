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

    public async Task SaveChangesAsync()
    {
        await Console.Out.WriteLineAsync("Data should be saved!");
    }
}
