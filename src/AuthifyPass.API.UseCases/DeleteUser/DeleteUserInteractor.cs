namespace AuthifyPass.API.UseCases.DeleteUser;

internal class DeleteUserInteractor(IUserSecretRepository repository) : IDeleteUserInputPort
{
    public async Task<bool> Handle(DeleteDto data)
    {
        bool result = false;
        UserSecret client = await repository.GetByClientIdAndSharedSecretAsync(data.ClientId, data.SharedSecret);
        if (client is not null)
        {
            if (client.ActiveSharedSecret.Equals(data.SharedSecret))
            {
                await repository.DeleteAsync(data);
                result = true;
            }
        }
        return result;
    }
}
