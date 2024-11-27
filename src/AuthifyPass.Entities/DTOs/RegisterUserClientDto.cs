namespace AuthifyPass.Entities.DTOs;
public class RegisterUserClientDto(string clientId, string userId)
{
    public string ClientId => clientId;

    public string UserId => userId;
}
