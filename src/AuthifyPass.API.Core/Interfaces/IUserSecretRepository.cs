namespace AuthifyPass.API.Core.Interfaces;
/// <summary>
/// Interface for managing UserSecret entities in the database.
/// </summary>
public interface IUserSecretRepository
{
    Task<UserSecret?> GetByUserIdAndSharedSecretAsync(string userId, string sharedSecret);
    Task<IQueryable<UserSecret>?> GetByClientIdAndSharedSecretAsync(string clientId, string sharedSecret);
    Task AddAsync(UserSecret userSecret);
    Task DeleteAsync(DeleteDto data);
}
