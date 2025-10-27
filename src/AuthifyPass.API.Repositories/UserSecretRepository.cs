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

    public async Task DeleteUserByClientSecretAsync(string hashedUserId, string clientSharedSecret)
    {
        IEnumerable<UserSecretEntity> users = await dbReader.GetUsersByIdAndClientSharedSecretAsync(hashedUserId, clientSharedSecret);
        if (users == null || !users.Any())
        {
            return;
        }

        foreach (UserSecretEntity user in users)
        {
            await dbWriter.DeleteUserSecretAsync(user);
        }
        await dbWriter.SaveChangesAsync();
    }


    public async Task<IQueryable<UserSecret>?> GetByUserByIdAndClientSharedSecretAsync(string userId, string sharedSecret)
    {
        var users = await dbReader.GetUsersByIdAndClientSharedSecretAsync(identifierGenerator.ComputeSha256Hash(userId), sharedSecret);
        return users.Select(CreateUserSecret).AsQueryable();
    }

    public async Task<IQueryable<UserSecret>?> GetByUserByIdAndSharedSecretAsync(string userId, string sharedSecret)
    {
        var users = await dbReader.GetUsersByIdAndSharedSecretAsync(identifierGenerator.ComputeSha256Hash(userId), sharedSecret);
        return users.Select(CreateUserSecret).AsQueryable();
    }

    private UserSecret? CreateUserSecret(UserSecretEntity? user)
    {
        UserSecret result = default;
        if (user is not null)
            result = new UserSecret(user.ClientId, user.UserId, user.ActiveSharedSecret);
        return result;
    }
}
