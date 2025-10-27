namespace AuthifyPass.API.UseCases.DeleteUser;

internal class DeleteUserInteractor(
    IUserSecretRepository repository,
    IClientRepository clientRepository,
    IIdentifierGenerator identifierGenerator) : IDeleteUserInputPort
{
    public async Task<bool> Handle(DeleteDto data)
    {
        bool result = false;
        try
        {
            bool isClientSecret = IsClientSecret(data.SharedSecret);

            if (isClientSecret)
            {
                await repository.DeleteUserByClientSecretAsync(identifierGenerator.ComputeSha256Hash(data.Id), data.SharedSecret);
            }
            else
            {
                await DeleteUsingUserSecret(data.Id, data.SharedSecret);
            }

            result = true;
        }
        catch
        {
            result = false;
        }
        return result;
    }

    private async Task DeleteUsingUserSecret(string userId, string userSharedSecret)
    {
        var client = await clientRepository.GetClientByUserIdAsync(identifierGenerator.ComputeSha256Hash(userId), userSharedSecret);
        if (client != null)
        {
            await repository.DeleteUserByClientSecretAsync(identifierGenerator.ComputeSha256Hash(userId), client.SharedSecret);
        }
    }

    private bool IsClientSecret(string secret)
    {
        bool result = false;
        if (!string.IsNullOrWhiteSpace(secret))
        {
            result = secret.Length == 64 && secret.All(IsHexadecimal);
        }
        return result;
    }

    private bool IsHexadecimal(char c)
    {
        return (c >= '0' && c <= '9') ||
               (c >= 'a' && c <= 'f') ||
               (c >= 'A' && c <= 'F');
    }
}

