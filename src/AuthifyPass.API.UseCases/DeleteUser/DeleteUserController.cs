namespace AuthifyPass.API.UseCases.DeleteUser;
internal class DeleteUserController(IDeleteUserInputPort input) : IDeleteUserController
{
    public async Task<IResult> DeleteUser(string clientId, string sharedSecret)
    {
        DeleteDto data = new(clientId, sharedSecret);
        bool isDeleted = await input.Handle(data);
        IResult result = isDeleted ? Results.NoContent() : Results.NotFound($"Client {clientId} and shared secret not found.");
        return result;
    }
}
