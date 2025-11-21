namespace AuthifyPass.API.UseCases.DeleteUser;
internal class DeleteUserController(IDeleteUserInputPort input) : IDeleteUserController
{
    public async Task<IResult> DeleteUser(string userId, string sharedSecret)
    {
        DeleteDto data = new(userId, sharedSecret);
        bool isDeleted = await input.Handle(data);
        IResult result = isDeleted ? Results.NoContent() : Results.NotFound($"User not found.");
        return result;
    }
}
