namespace AuthifyPass.API.Core.Interfaces.UseCases.DeleteClient;
public interface IDeleteClientInputPort
{
    Task<bool> Handle(DeleteDto data);
}
