namespace AuthifyPass.API.Core.Interfaces.UseCases.DeleteClient;
public interface IDeleteClientController
{
    Task<IResult> DeleteClient(string clientId, string sharedSecret);
}
