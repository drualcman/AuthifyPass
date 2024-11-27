namespace AuthifyPass.Entities.DTOs;
public class ValidateUserCodeDto(string clientId, string userId, string userCode)
{
    public string ClientId => clientId;
    public string UserId => userId;
    public string UserCode => userCode;
}
