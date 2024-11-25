using AuthifyPass.API.Core.DTOs;
using AuthifyPass.API.Core.Interfaces.UseCases.RegisterClient;
using AuthifyPass.API.UseCases.Resources;
using Microsoft.Extensions.Localization;

namespace AuthifyPass.API.UseCases.RegisterClient;
internal class RegisterClientPresenter(IStringLocalizer<RegisterClientContent> localizer) : IRegisterClientOutputPort
{
    public RegisterClientResultDto Content { get; private set; }

    public Task Handle(string name, string clientId, string sharedSecret)
    {
        string message = string.Format(localizer[nameof(RegisterClientContent.CreateMessageTemplate)], name);
        Content = new(clientId, sharedSecret, message);
        return Task.CompletedTask;
    }
}
