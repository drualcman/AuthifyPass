namespace AuthifyPass.API.Core.Interfaces;
/// <summary>
/// Interface for managing UserSecret entities in the database.
/// </summary>
public interface IUserSecretRepository
{
    Task<IQueryable<UserSecret>?> GetByClientIdAndUserIdAsync(string clientId, string userId);
    Task<UserSecret?> GetByClientIdAndSharedSecretAsync(string clientId, string sharedSecret);
    Task AddAsync(UserSecret userSecret);
    Task DeleteAsync(DeleteDto data);
}
