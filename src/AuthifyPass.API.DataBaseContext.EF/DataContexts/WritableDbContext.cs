namespace AuthifyPass.API.DataBaseContext.EF.DataContexts;
internal class WritableDbContext(IOptions<DataBaseOptions> dbOptions) : AuthifyPassDbContext, IWritableDbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.UseSqlServer(dbOptions.Value.Writable);
        base.OnConfiguring(optionsBuilder);
    }

    public async Task AddClientAsync(ClientEntity client) => await Clients.AddAsync(client);

    public async Task AddUserSecretAsync(UserSecretEntity userSecret) => await Users.AddAsync(userSecret);

    public async Task DeleteClientAsync(string clientId)
    {
        var client = await Clients.FirstAsync(x => x.ClientId.Equals(clientId));
        if (client != null)
            Clients.Remove(client);
    }

    public Task UpdateUserSecretAsync(UserSecretEntity userSecret)
    {
        Users.Update(userSecret);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync() => await base.SaveChangesAsync();
}
