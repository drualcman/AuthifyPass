namespace AuthifyPass.API.UseCases.ValidateUserCode;
internal class ValidateUserCodeInteractor(
    IIdentifierGenerator identifierGenerator,
    IUserSecretRepository userRepository,
    IClientRepository clientRepository,
    IStringCulture<RegisterUserContent> localizer,
    IModelValidatorHub<ValidateUserCodeDto> validator) : IValidateUserCodeInputPort
{
    public async Task<bool> ValidateUserCode(ValidateUserCodeDto data, string secretShared)
    {
        await GuardModel.AgainstNotValid(validator, data);
        bool result = false;
        var client = await clientRepository.GetClientByIdAsync(data.ClientId);
        ThrowIfNullOrNotValid(secretShared, client);
        var users = await userRepository.GetByClientIdAndUserIdAsync(data.ClientId, identifierGenerator.ComputeSha256Hash(data.UserId));

        if (users is null)
            throw new KeyNotFoundException(localizer[nameof(RegisterUserContent.InvalidUser)]);

        var user = users?
            .Where(user => TOTPHelper.ValidateTOTP(data.UserCode, user.ActiveSharedSecret))
            .FirstOrDefault() ?? null;

        result = user is not null;
        return result;
    }

    private void ThrowIfNullOrNotValid(string secretShared, Client? client)
    {
        if (client == null)
        {
            throw new UnauthorizedAccessException(localizer[nameof(RegisterUserContent.ClientUnauthorized)]);
        }
        if (string.IsNullOrEmpty(client.SharedSecret) || !client.SharedSecret.Equals(secretShared))
        {
            throw new UnauthorizedAccessException(localizer[nameof(RegisterUserContent.ClientUnauthorized)]);
        }
    }
}
