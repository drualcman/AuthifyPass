using AuthifyPass.API.Core.DTOs;
using AuthifyPass.API.Core.Guards;
using AuthifyPass.API.Core.Interfaces;
using AuthifyPass.API.Core.Interfaces.UseCases.RegisterClient;
using AuthifyPass.Entities.DTOs;
using AuthifyPass.Entities.Interfaces;

namespace AuthifyPass.API.UseCases.RegisterClient;
internal class RegisterClientInteractor(IClientRepository repository,
    IModelValidatorHub<RegisterClientDto> validator,
    IRegisterClientOutputPort output,
    IIdentifierGenerator identifierGenerator) : IRegisterClientInputPort
{
    public async Task Handle(RegisterClientDto register)
    {
        await GuardModel.AgainstNotValid(validator, register);

        string clientId = identifierGenerator.GenerateClientId();
        string sharedSecret = identifierGenerator.GenerateSharedSecret();
        AddClientDto client = new(
            clientId: clientId,
            name: register.Name,
            email: register.Email,
            password: register.Password,
            sharedSecret: sharedSecret
            );
        await repository.AddClientAsync(client);
        await output.Handle(register.Name, clientId, sharedSecret);
    }
}
