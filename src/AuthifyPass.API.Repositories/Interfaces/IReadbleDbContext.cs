namespace AuthifyPass.API.Repositories.Interfaces;
public interface IReadbleDbContext
{
    Task<ClientEntity?> GetClientByIdAsync(string clientId, string sharedSecret);
    Task<ClientEntity?> GetClientByEmailAsync(string clientId, string sharedSecret);
    Task<UserSecretEntity?> GetByUserIdAndSharedSecretAsync(string userId, string sharedSecret);
    Task<IEnumerable<UserSecretEntity>?> GetAllUsersByIdAndSharedAsync(string userId);
    Task<IEnumerable<UserSecretEntity>?> GetByClientIdAndSaredSecretAsync(string clientId, string sharedSecret);
}
