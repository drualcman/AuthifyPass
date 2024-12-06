namespace AuthifyPass.Entities.DTOs;

public class DeleteDto(string clientId, string sharedSecret)
{
    public string ClientId => clientId;
    public string SharedSecret => sharedSecret;
}
