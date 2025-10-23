using AuthifyPass.Entities.Helpers;
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
        ThrowIfNotValidSecret(register.Code);
        await ThrowIfEmailExists(register.Email, register.Code);
        string clientId = identifierGenerator.GenerateClientId();
        string sharedSecret = identifierGenerator.GenerateSharedSecret();
        AddClientDto client = CreateClient(register, clientId, sharedSecret);
        await repository.AddClientAsync(client);
        await output.Handle(register.Name, clientId, sharedSecret);
    }

    void ThrowIfNotValidSecret(string sharedSecred)
    {
        string secret = configuration.GetValue<string>("Secret");
        if (!TOTPGeneratorHelper.ValidateTOTP(sharedSecred, secret))
            throw new ValidationException(new List<ValidationError>()
            {
                new ValidationError(nameof(RegisterClientDto.Code), localizer[nameof(DatabaseErrors.ValidationCodeError)])
            });
    }

    private async Task ThrowIfEmailExists(string email, string secret)
    {
        Client existingClient = await repository.GetClientByEmailAsync(email, secret);
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
