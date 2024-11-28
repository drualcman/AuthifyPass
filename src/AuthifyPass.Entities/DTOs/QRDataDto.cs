namespace AuthifyPass.Entities.DTOs;
public class QRDataDto(string name, string clientId, string sharedKey)
{
    public string Name => name;
    public string ClientId => clientId;
    public string SharedKey => sharedKey;
}
