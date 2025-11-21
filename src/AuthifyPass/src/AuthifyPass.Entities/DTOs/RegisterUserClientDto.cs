namespace AuthifyPass.Entities.DTOs;
public class RegisterUserClientDto(string clientId, string userId, string userName)
{
    public string ClientId => clientId;
    public string UserId => userId;
    public string UserName => userName;
}
