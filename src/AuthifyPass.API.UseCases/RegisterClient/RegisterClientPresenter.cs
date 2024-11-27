﻿namespace AuthifyPass.API.UseCases.RegisterClient;
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
