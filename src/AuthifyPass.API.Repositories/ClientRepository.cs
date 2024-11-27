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
        await dbWriter.SaveChangesAsync();
    }

    public Task DeleteClientAsync(string clientId)
    {
        return Task.CompletedTask;
    }

    public async Task<Client?> GetByClientIdAsync(string clientId)
    {
        var client = await dbReader.GetByClientIdAsync(clientId);
        Client result = default;
        if (client != null)
            result = new Client(client?.ClientId, client?.SharedSecret,
                client?.Name, client?.Email, client?.Password,
                client?.LastUpdateAt ?? DateTime.UtcNow);
        return result;
    }
}
