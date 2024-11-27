namespace AuthifyPass.API.Core.Interfaces.UseCases.RegisterClient;
public interface IRegisterClientOutputPort
{
    RegisterClientResultDto Content { get; }
    Task Handle(string name, string clientId, string sharedSecret);
}
