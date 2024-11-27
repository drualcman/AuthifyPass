namespace AuthifyPass.API.Core.Interfaces;
public interface IClientRepository
{
    Task<Client?> GetByClientIdAsync(string clientId);
    Task AddClientAsync(AddClientDto client);
    Task DeleteClientAsync(string clientId);
}
