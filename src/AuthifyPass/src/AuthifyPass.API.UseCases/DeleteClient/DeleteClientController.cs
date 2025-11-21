namespace AuthifyPass.API.UseCases.DeleteClient;
internal class DeleteClientController(IDeleteClientInputPort input) : IDeleteClientController
{
    public async Task<IResult> DeleteClient(string clientId, string sharedSecret)
    {
        DeleteDto data = new(clientId, sharedSecret);
        bool isDeleted = await input.Handle(data);
        IResult result = isDeleted ? Results.NoContent() : Results.NotFound($"Client {clientId} and shared secret not found.");
        return result;
    }
}
