namespace AuthifyPass.API.Core.Interfaces.UseCases.DeleteUser;
public interface IDeleteUserInputPort
{
    Task<bool> Handle(DeleteDto data);
}
