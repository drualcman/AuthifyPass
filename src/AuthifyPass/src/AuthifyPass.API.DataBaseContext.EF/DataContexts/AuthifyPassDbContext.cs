namespace AuthifyPass.API.DataBaseContext.EF.DataContexts;
internal class AuthifyPassDbContext : DbContext
{
    public DbSet<ClientEntity> Clients { get; set; }
    public DbSet<UserSecretEntity> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ClientEntityConfiguration());
        modelBuilder.ApplyConfiguration(new UserSecretEntityConfiguration());
    }
}
