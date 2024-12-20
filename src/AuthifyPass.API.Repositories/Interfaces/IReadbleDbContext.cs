namespace AuthifyPass.API.Repositories.Interfaces;
public interface IReadbleDbContext
{
    Task<ClientEntity?> GetByClientIdAsync(string clientId);
    Task<IEnumerable<UserSecretEntity>?> GetByClientIdAndUserIdAsync(string clientId, string userId);
    Task<UserSecretEntity?> GetByClientIdAndSaredSecretAsync(string clientId, string sharedSecret);
}
