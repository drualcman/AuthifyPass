namespace AuthifyPass.API.Core.Interfaces;
/// <summary>
/// Interface for managing UserSecret entities in the database.
/// </summary>
public interface IUserSecretRepository
{
    Task<IQueryable<UserSecret>?> GetByUserByIdAndClientSharedSecretAsync(string userId, string sharedSecret);
    Task<IQueryable<UserSecret>?> GetByUserByIdAndSharedSecretAsync(string userId, string sharedSecret);
    Task AddAsync(UserSecret userSecret);
    Task DeleteUserByClientSecretAsync(string hashedUserId, string clientSharedSecret);
}
