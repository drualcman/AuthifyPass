﻿namespace AuthifyPass.API.Core.Interfaces;
public interface IWritableDbContext
{
    Task AddClientAsync(ClientEntity client);
    Task DeleteClientAsync(string clientId);
    Task AddUserSecretAsync(UserSecretEntity userSecret);
    Task UpdateUserSecretAsync(UserSecretEntity userSecret);
    Task DeleteUserSecretAsync(UserSecretEntity userSecret);
    Task SaveChangesAsync();
    Task MigrateAsync();
}
