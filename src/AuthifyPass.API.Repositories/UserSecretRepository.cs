namespace AuthifyPass.API.Repositories;
internal class UserSecretRepository(
    IIdentifierGenerator identifierGenerator,
    IWritableDbContext dbWriter, IReadbleDbContext dbReader) : IUserSecretRepository
{
    public async Task AddAsync(UserSecret userSecret)
    {
        UserSecretEntity entity = new UserSecretEntity
        {
            ClientId = userSecret.ClientId,
            UserId = identifierGenerator.ComputeSha256Hash(userSecret.UserId),
            ActiveSharedSecret = userSecret.ActiveSharedSecret,
            CreatetAt = DateTime.UtcNow
        };
        await dbWriter.AddUserSecretAsync(entity);
        await dbWriter.SaveChangesAsync();
    }

    public async Task DeleteAsync(DeleteDto data)
    {
        var user = await dbReader.GetByUserIdAndSharedSecretAsync(identifierGenerator.ComputeSha256Hash(data.Id), data.SharedSecret);
        if (user is not null)
        {
            await dbWriter.DeleteUserSecretAsync(user);
            await dbWriter.SaveChangesAsync();
        }
    }

    public async Task<IQueryable<UserSecret>?> GetByClientIdAndSharedSecretAsync(string clientId, string sharedSecret)
    {
        var users = await dbReader.GetByClientIdAndSaredSecretAsync(clientId, sharedSecret);
        return users.Select(CreateUserSecret).AsQueryable();
    }

    public async Task<UserSecret?> GetByUserIdAndSharedSecretAsync(string userId, string sharedSecret)
    {
        var user = await dbReader.GetByUserIdAndSharedSecretAsync(identifierGenerator.ComputeSha256Hash(userId), sharedSecret);
        return CreateUserSecret(user);
    }
    private UserSecret? CreateUserSecret(UserSecretEntity? user)
    {
        UserSecret result = default;
        if (user != null)
            result = new UserSecret(user.ClientId, user.UserId, user.ActiveSharedSecret);
        return result;
    }
}
