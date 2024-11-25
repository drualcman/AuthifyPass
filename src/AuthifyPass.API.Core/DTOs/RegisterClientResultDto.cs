namespace AuthifyPass.API.Core.DTOs;
public class RegisterClientResultDto(string? clientId, string? sharedSecret, string message)
{
    public string? ClientId => clientId;
    public string? SharedSecret => sharedSecret;
    public string Message => message;
}
