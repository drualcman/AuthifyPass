using Microsoft.Extensions.Configuration;

namespace AuthifyPass.API.UseCases.RegisterClient;
internal class RegisterClientInteractor(IClientRepository repository,
    IModelValidatorHub<RegisterClientDto> validator,
    IRegisterClientOutputPort output,
    IStringCulture<DatabaseErrors> localizer,
    IConfiguration configuration,
    IIdentifierGenerator identifierGenerator) : IRegisterClientInputPort
{
    public async Task Handle(RegisterClientDto register)
    {
        await GuardModel.AgainstNotValid(validator, register);
        ThrowIfNotValidCode(register.Code);
        await ThrowIfEmailExists(register.Email);
        string clientId = identifierGenerator.GenerateClientId();
        string sharedSecret = identifierGenerator.GenerateSharedSecret();
        AddClientDto client = CreateClient(register, clientId, sharedSecret);
        await repository.AddClientAsync(client);
        await output.Handle(register.Name, clientId, sharedSecret);
    }

    void ThrowIfNotValidCode(string code)
    {
        string secret = configuration.GetValue<string>("Secret");
        if (!TOTPHelper.ValidateTOTP(code, secret))
            throw new ValidationException(new List<ValidationError>()
            {
                new ValidationError(nameof(RegisterClientDto.Code), localizer[nameof(DatabaseErrors.ValidationCodeError)])
            });
    }

    private async Task ThrowIfEmailExists(string email)
    {
        Client existingClient = await repository.GetClientByEmailAsync(email);
        if (existingClient is not null)
            throw new ValidationException(new List<ValidationError>()
            {
                new ValidationError(nameof(RegisterClientDto.Email),
                                    string.Format(localizer.GetString(nameof(DatabaseErrors.DuplicateEmailErrorTemplate)), email))
            });
    }

    private AddClientDto CreateClient(RegisterClientDto register, string clientId, string sharedSecret)
    {
        return new(
                    clientId: clientId,
                    name: register.Name,
                    email: register.Email,
                    password: identifierGenerator.ComputeSha256Hash(register.Password),
                    sharedSecret: sharedSecret
                    );
    }
}
