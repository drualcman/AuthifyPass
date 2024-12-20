
namespace AuthifyPass.API.Repositories;
internal class UserSecretRepository(IWritableDbContext dbWriter, IReadbleDbContext dbReader) : IUserSecretRepository
{
    public async Task AddAsync(UserSecret userSecret)
    {
        UserSecretEntity entity = new UserSecretEntity
        {
            ClientId = userSecret.ClientId,
            UserId = userSecret.UserId,
            ActiveSharedSecret = userSecret.ActiveSharedSecret,
            CreatetAt = DateTime.UtcNow
        };
        await dbWriter.AddUserSecretAsync(entity);
        await dbWriter.SaveChangesAsync();
    }

    public async Task DeleteAsync(DeleteDto data)
    {
        var user = await dbReader.GetByClientIdAndSaredSecretAsync(data.ClientId, data.SharedSecret);
        if (user is not null)
        {
            await dbWriter.DeleteUserSecretAsync(user);
            await dbWriter.SaveChangesAsync();
        }
    }

    public async Task<UserSecret?> GetByClientIdAndSharedSecretAsync(string clientId, string sharedSecret)
    {
        var user = await dbReader.GetByClientIdAndSaredSecretAsync(clientId, sharedSecret);
        return CreateUserSecret(user);
    }

    public async Task<IQueryable<UserSecret>?> GetByClientIdAndUserIdAsync(string clientId, string userId)
    {
        var users = await dbReader.GetByClientIdAndUserIdAsync(clientId, userId);
        return users.Select(CreateUserSecret).AsQueryable();
    }
    private UserSecret? CreateUserSecret(UserSecretEntity? user)
    {
        UserSecret result = default;
        if (user != null)
            result = new UserSecret(user.ClientId, user.ClientId, user.ActiveSharedSecret);
        return result;
    }
}
