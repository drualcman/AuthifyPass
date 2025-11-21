namespace AuthifyPass.API.UseCases.RegisterClient;
internal class RegisterClientController(IRegisterClientInputPort input,
    IRegisterClientOutputPort presenter) : IRegisterClientController
{
    public async Task<RegisterClientResponseDto> CreateClientAsync(RegisterClientDto client)
    {
        await input.Handle(client);
        return presenter.Content;
    }
}
