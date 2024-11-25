using AuthifyPass.API.Core.DTOs;
using AuthifyPass.API.Core.Interfaces.UseCases.RegisterClient;
using AuthifyPass.Entities.DTOs;

namespace AuthifyPass.API.UseCases.RegisterClient;
internal class RegisterClientController(IRegisterClientInputPort input, 
    IRegisterClientOutputPort presenter) : IRegisterClientController
{
    public async Task<RegisterClientResultDto> CreateClientAsync(RegisterClientDto client)
    {
        await input.Handle(client);
        return presenter.Content;
    }
}
