namespace AuthifyPass.API.Core.Interfaces.UseCases.DeleteClient;
public interface IDeleteUserController
{
    Task<IResult> DeleteUser(string clientId, string sharedSecret);
}
