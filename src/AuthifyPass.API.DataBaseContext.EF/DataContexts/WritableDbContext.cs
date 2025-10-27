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

    public async Task DeleteClientAsync(string clientId, string shared)
    {
        var client = await Clients.FirstAsync(x => x.ClientId == clientId && x.SharedSecret == shared);
        if (client is not null)
            Clients.Remove(client);
    }

    public Task DeleteUserSecretAsync(UserSecretEntity userSecret)
    {
        var user = Users.FirstOrDefault(x => x.UserId == userSecret.UserId && x.ClientId == userSecret.ClientId && x.ActiveSharedSecret == userSecret.ActiveSharedSecret);
        if (user is not null)
            Users.Remove(user);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync() => await base.SaveChangesAsync();

    public async Task MigrateAsync()
    {
        await Database.MigrateAsync();
    }
}
