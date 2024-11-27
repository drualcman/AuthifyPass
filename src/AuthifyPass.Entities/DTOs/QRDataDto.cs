namespace AuthifyPass.Entities.DTOs;
public class QRDataDto(string clientId, string sharedKey)
{
    public string ClientId => clientId;
    public string SharedKey => sharedKey;
}
