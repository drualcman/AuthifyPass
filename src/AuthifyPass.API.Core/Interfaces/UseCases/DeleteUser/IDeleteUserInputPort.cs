namespace AuthifyPass.API.Core.Interfaces.UseCases.DeleteClient;
public interface IDeleteUserInputPort
{
    Task<bool> Handle(DeleteDto data);
}
