namespace AuthifyPass.API.DataBaseContext.EF.DataContexts;
internal class ReadableDbContext(IOptions<DataBaseOptions> dbOptions) : AuthifyPassDbContext, IReadbleDbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        optionsBuilder.UseSqlServer(dbOptions.Value.Readable);
        base.OnConfiguring(optionsBuilder);
    }

    public async Task<UserSecretEntity?> GetByClientIdAndUserIdAsync(string clientId, string userId) =>
        await Users.FirstAsync(c => c.ClientId.Equals(clientId) && c.UserId.Equals(userId));

    public async Task<ClientEntity?> GetByClientIdAsync(string clientId) =>
        await Clients.FirstAsync(c => c.ClientId.Equals(clientId));
}
