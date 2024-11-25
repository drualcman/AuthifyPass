namespace AuthifyPass.API.Core.DTOs;

public class AddClientDto(string? clientId, string? name, string? email, string? password, string? sharedSecret)
{
    public string? ClientId => clientId;
    public string? Name => name;
    public string? Email => email;
    public string? Password => password;
    public string? SharedSecret => sharedSecret;
}

