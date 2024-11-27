namespace AuthifyPass.Entities.DTOs;

public class DeleteClientDto(string clientId, string sharedSecret)
{
    public string ClientId => clientId;
    public string SharedSecret => sharedSecret;
}
