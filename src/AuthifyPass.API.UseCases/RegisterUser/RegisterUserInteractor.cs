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
        var client = await clientRepository.GetClientByIdAsync(data.ClientId, sharedSecret);
        if (client is not null && client.SharedSecret.Equals(sharedSecret))
        {
            string sharedkey = identifierGenerator.GenerateSharedSecret();
            UserSecret userSecret = new(data.ClientId, data.UserId, sharedkey);
            await userRepository.AddAsync(userSecret);
            response = new(sharedkey, qrGenerator.GenerateQRCode(new(client.Name, data.UserName, data.UserId, sharedkey)));
        }
        else
            throw new UnauthorizedAccessException(localizer[nameof(RegisterUserContent.ClientUnauthorized)]);
        return response;
    }
}
