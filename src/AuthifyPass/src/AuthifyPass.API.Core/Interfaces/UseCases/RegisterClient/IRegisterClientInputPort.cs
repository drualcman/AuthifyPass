namespace AuthifyPass.API.Core.Interfaces.UseCases.RegisterClient;
public interface IRegisterClientInputPort
{
    Task Handle(RegisterClientDto client);
}
