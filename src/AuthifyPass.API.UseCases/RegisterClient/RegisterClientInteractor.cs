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
            password: identifierGenerator.ComputeSha256Hash(register.Password),
            sharedSecret: sharedSecret
            );
        await repository.AddClientAsync(client);
        await repository.SaveChangesAsync();
        await output.Handle(register.Name, clientId, sharedSecret);
    }
}
