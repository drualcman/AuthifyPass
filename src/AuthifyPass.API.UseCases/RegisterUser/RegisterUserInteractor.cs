namespace AuthifyPass.API.UseCases.RegisterUser;
internal class RegisterUserInteractor(
    IIdentifierGenerator identifierGenerator,
    IUserSecretRepository userRepository,
    IClientRepository clientRepository,
    IModelValidatorHub<RegisterUserClientDto> validator,
    IStringCulture<RegisterUserContent> localizer,
    IQRGeneratorRepository qrGenerator) : IRegisterUserInputPort
{
    public async Task<RegisterUserClientResponseDto> RegisterUserAsync(RegisterUserClientDto data, string sharedSecret)
    {
        await GuardModel.AgainstNotValid(validator, data);
        RegisterUserClientResponseDto response = default;
        var client = await clientRepository.GetClientByIdAsync(data.ClientId);
        if (client is not null && client.SharedSecret.Equals(sharedSecret))
        {
            string sharedkey = identifierGenerator.GenerateSharedSecret();
            string userId = identifierGenerator.ComputeSha256Hash(data.UserId);
            UserSecret userSecret = new(data.ClientId, userId, sharedkey);
            await userRepository.AddAsync(userSecret);
            response = new(sharedkey, qrGenerator.GenerateQRCode(new(client.Name, data.ClientId, sharedkey)));
        }
        else
            throw new UnauthorizedAccessException(localizer[nameof(RegisterUserContent.ClientUnauthorized)]);
        return response;
    }
}
