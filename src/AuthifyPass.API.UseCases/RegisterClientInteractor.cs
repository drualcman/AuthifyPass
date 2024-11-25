using AuthifyPass.API.Core.DTOs;
using AuthifyPass.API.Core.Guards;
using AuthifyPass.API.Core.Interfaces;
using AuthifyPass.Entities.DTOs;
using AuthifyPass.Entities.Interfaces;

namespace AuthifyPass.API.UseCases;
internal class RegisterClientInteractor(IClientRepository repository,
    IModelValidatorHub<RegisterClientDto> validator) : IRegisterClientInputPort
{
    public async Task CreateClientAsync(RegisterClientDto register)
    {
        await GuardModel.AgainstNotValid(validator, register);

        string clientId = Guid.NewGuid().ToString("N");
        string sharedSecret = GenerateSharedSecret();
        AddClientDto client = new(
            clientId: clientId,
            name: register.Name,
            email: register.Email,
            password: register.Password,
            sharedSecret: sharedSecret
            );
        await repository.AddClientAsync(client);
    }


    private string GenerateSharedSecret()
    {
        // Replace with a robust secret generation logic
        return Guid.NewGuid().ToString("N").Substring(0, 32);
    }
}
