namespace AuthifyPass.API.Core.Interfaces;
public interface IClientRepository
{
    Task<Client?> GetClientByUserIdAsync(string userId, string sharedSecret);
    Task<Client?> GetClientByIdAsync(string clientId, string sharedSecret);
    Task<Client?> GetClientByEmailAsync(string email, string sharedSecret);
    Task AddClientAsync(AddClientDto client);
    Task DeleteClientAsync(string clientId, string sharedSecret);
}
