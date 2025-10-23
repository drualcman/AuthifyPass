namespace AuthifyPass.API.UseCases.DeleteClient;
internal class DeleteClientInteractor(IClientRepository repository) : IDeleteClientInputPort
{
    public async Task<bool> Handle(DeleteDto data)
    {
        bool result = false;
        Client client = await repository.GetClientByIdAsync(data.Id, data.SharedSecret);
        if (client is not null)
        {
            if (client.SharedSecret.Equals(data.SharedSecret))
            {
                await repository.DeleteClientAsync(data.Id, data.SharedSecret);
                result = true;
            }
        }
        return result;
    }
}
