namespace AuthifyPass.Entities.DTOs;
public class QRDataDto(string appName, string name, string clientId, string sharedKey)
{
    public string AppName => appName;
    public string Name => name;
    public string ClientId => clientId;
    public string SharedKey => sharedKey;
}
