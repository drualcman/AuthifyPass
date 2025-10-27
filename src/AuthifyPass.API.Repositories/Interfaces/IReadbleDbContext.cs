namespace AuthifyPass.API.Repositories.Interfaces;
public interface IReadbleDbContext
{
    Task<ClientEntity?> GetClientByUserIdAsync(string userId, string sharedSecret);
    Task<ClientEntity?> GetClientByIdAsync(string clientId, string sharedSecret);
    Task<ClientEntity?> GetClientByEmailAsync(string clientId, string sharedSecret);
    Task<IEnumerable<UserSecretEntity>?> GetUsersByIdAndClientSharedSecretAsync(string userId, string sharedSecret);
    Task<IEnumerable<UserSecretEntity>?> GetUsersByIdAndSharedSecretAsync(string userId, string sharedSecret);
    Task<IEnumerable<UserSecretEntity>?> GetUsersByClientIdAndSaredSecretAsync(string clientId, string sharedSecret);
}
