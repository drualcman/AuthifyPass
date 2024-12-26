namespace AuthifyPass.API.DataBaseContext.EF.DataContexts;
internal class ReadableDbContext(IOptions<DataBaseOptions> dbOptions) : AuthifyPassDbContext, IReadbleDbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        optionsBuilder.UseSqlServer(dbOptions.Value.Readable);
        base.OnConfiguring(optionsBuilder);
    }

    public async Task<IEnumerable<UserSecretEntity>?> GetByClientIdAndUserIdAsync(string clientId, string userId) =>
        await Users.Where(c => c.ClientId.Equals(clientId) && c.UserId.Equals(userId)).ToListAsync();

    public async Task<UserSecretEntity?> GetByClientIdAndSaredSecretAsync(string clientId, string sharedSecret) =>
        await Users.FirstOrDefaultAsync(c => c.ClientId.Equals(clientId) && c.ActiveSharedSecret.Equals(sharedSecret));

    public async Task<ClientEntity?> GetClientByIdAsync(string clientId) =>
        await Clients.FirstOrDefaultAsync(c => c.ClientId.Equals(clientId));
    public async Task<ClientEntity?> GetClientByEmailAsync(string email) =>
        await Clients.FirstOrDefaultAsync(c => c.Email.Equals(email));
}
