namespace AuthifyPass.API.UseCases.DeleteUser;

internal class DeleteUserInteractor(IUserSecretRepository repository) : IDeleteUserInputPort
{
    public async Task<bool> Handle(DeleteDto data)
    {
        bool result = false;
        var client = await repository.GetByUserIdAndSharedSecretAsync(data.Id, data.SharedSecret);
        if (client is not null)
        {
            await repository.DeleteAsync(data);
        }
        return result;
    }
}
