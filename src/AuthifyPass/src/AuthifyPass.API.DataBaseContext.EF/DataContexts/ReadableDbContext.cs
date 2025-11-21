namespace AuthifyPass.API.DataBaseContext.EF.DataContexts;
internal class ReadableDbContext(IOptions<DataBaseOptions> dbOptions) : AuthifyPassDbContext, IReadbleDbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        optionsBuilder.UseSqlServer(dbOptions.Value.Readable);
        base.OnConfiguring(optionsBuilder);
    }

    public async Task<IEnumerable<UserSecretEntity>?> GetUsersByIdAndClientSharedSecretAsync(string userId, string sharedSecret) =>
        await Users
        .Include(u => u.Client)
        .Where(c => c.UserId == userId && c.Client.SharedSecret == sharedSecret).ToListAsync();

    public async Task<IEnumerable<UserSecretEntity>?> GetUsersByIdAndSharedSecretAsync(string userId, string sharedSecret) =>
        await Users
        .Where(c => c.UserId == userId && c.ActiveSharedSecret == sharedSecret).ToListAsync();

    public async Task<IEnumerable<UserSecretEntity>?> GetUsersByClientIdAndSaredSecretAsync(string clientId, string sharedSecret) =>
        await Users
        .Include(u => u.Client)
        .Where(c => c.ClientId == clientId && c.Client.SharedSecret == sharedSecret).ToListAsync();

    public async Task<ClientEntity?> GetClientByUserIdAsync(string userId, string sharedSecret) =>
        await Users
        .Include(u => u.Client)
        .Where(c => c.UserId == userId && c.ActiveSharedSecret == sharedSecret)
        .Select(c => c.Client)
        .FirstOrDefaultAsync();

    public async Task<ClientEntity?> GetClientByIdAsync(string clientId, string sharedSecret) =>
        await Clients.FirstOrDefaultAsync(c => c.ClientId == clientId && c.SharedSecret == sharedSecret);
    public async Task<ClientEntity?> GetClientByEmailAsync(string email, string sharedSecret) =>
        await Clients.FirstOrDefaultAsync(c => c.Email == email && c.SharedSecret == sharedSecret);
}
