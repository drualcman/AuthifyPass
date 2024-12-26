namespace AuthifyPass.API.Core.Interfaces;
public interface IClientRepository
{
    Task<Client?> GetClientByIdAsync(string clientId);
    Task<Client?> GetClientByEmailAsync(string email);
    Task AddClientAsync(AddClientDto client);
    Task DeleteClientAsync(string clientId);
}
