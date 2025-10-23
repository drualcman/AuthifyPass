namespace AuthifyPass.API.Core.Interfaces;
public interface IClientRepository
{
    Task<Client?> GetClientByIdAsync(string clientId, string sharedSecret);
    Task<Client?> GetClientByEmailAsync(string email, string sharedSecret);
    Task AddClientAsync(AddClientDto client);
    Task DeleteClientAsync(string clientId, string sharedSecret);
}
