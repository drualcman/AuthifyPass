namespace AuthifyPass.API.UseCases.RegisterUser;
internal class RegisterUserInteractor(
    IIdentifierGenerator identifierGenerator,
    IUserSecretRepository userRepository,
    IClientRepository clientRepository,
    IModelValidatorHub<RegisterUserClientDto> validator,
    IStringLocalizer<RegisterUserContent> localizer) : IRegisterUserInputPort
{
    public async Task<string> RegisterUserAsync(RegisterUserClientDto data, string sharedSecret)
    {
        await GuardModel.AgainstNotValid(validator, data);
        string userSharedKey = string.Empty;
        var client = await clientRepository.GetByClientIdAsync(data.ClientId);
        if (client is not null && !string.IsNullOrEmpty(client.SharedSecret) && client.SharedSecret.Equals(sharedSecret))
        {
            string sharedkey = identifierGenerator.GenerateSharedSecret();
            string userId = identifierGenerator.ComputeSha256Hash(data.UserId);
            var user = await userRepository.GetByClientIdAndUserIdAsync(client.ClientId, userId);
            UserSecret userSecret = new(data.ClientId, userId, sharedkey);
            if (user is null)
                await userRepository.AddAsync(userSecret);
            else
                await userRepository.UpdateAsync(userSecret);
            userSharedKey = sharedkey;
        }
        else
            throw new UnauthorizedAccessException(localizer[nameof(RegisterUserContent.ClientUnauthorized)]);
        return userSharedKey;
    }
}
