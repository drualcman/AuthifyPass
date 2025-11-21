namespace AuthifyPass.API.Core.Interfaces.UseCases.RegisterClient;
public interface IRegisterClientOutputPort
{
    RegisterClientResponseDto Content { get; }
    Task Handle(string name, string clientId, string sharedSecret);
}
