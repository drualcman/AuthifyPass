namespace AuthifyPass.API.Repositories.Interfaces;
public interface IReadbleDbContext
{
    Task<ClientEntity?> GetClientByIdAsync(string clientId);
    Task<ClientEntity?> GetClientByEmailAsync(string clientId);
    Task<IEnumerable<UserSecretEntity>?> GetByClientIdAndUserIdAsync(string clientId, string userId);
    Task<UserSecretEntity?> GetByClientIdAndSaredSecretAsync(string clientId, string sharedSecret);
}
