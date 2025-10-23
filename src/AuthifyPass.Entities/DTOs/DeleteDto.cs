namespace AuthifyPass.Entities.DTOs;

public class DeleteDto(string id, string sharedSecret)
{
    public string Id => id;
    public string SharedSecret => sharedSecret;
}
