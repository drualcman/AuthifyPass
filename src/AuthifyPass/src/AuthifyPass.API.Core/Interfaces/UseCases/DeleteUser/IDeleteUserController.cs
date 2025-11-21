namespace AuthifyPass.API.Core.Interfaces.UseCases.DeleteUser;
public interface IDeleteUserController
{
    Task<IResult> DeleteUser(string userId, string sharedSecret);
}
