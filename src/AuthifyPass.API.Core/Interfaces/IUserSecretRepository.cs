namespace AuthifyPass.API.Core.Interfaces;
/// <summary>
/// Interface for managing UserSecret entities in the database.
/// </summary>
public interface IUserSecretRepository
{
    Task<UserSecret?> GetByClientIdAndUserIdAsync(string clientId, string userId);
    Task AddAsync(UserSecret userSecret);
    Task UpdateAsync(UserSecret userSecret);
}
