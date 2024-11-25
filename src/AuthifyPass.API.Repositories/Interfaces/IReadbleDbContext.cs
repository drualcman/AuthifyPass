using AuthifyPass.API.Repositories.Entities;

namespace AuthifyPass.API.Repositories.Interfaces;
public interface IReadbleDbContext
{
    Task<ClientEntity?> GetByClientIdAsync(string clientId);
    Task<UserSecretEntity?> GetByClientIdAndUserIdAsync(string clientId, string userId);
}
