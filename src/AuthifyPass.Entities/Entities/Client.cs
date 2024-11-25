namespace AuthifyPass.Entities.Entities;
/// <summary>
/// Represents a client integrating the authenticator API.
/// </summary>
public class Client(string? clientId, string? sharedSecret, string? name, string? email, string? password, DateTime lastUpdateAt)
{
    public string? ClientId { get; set; } = clientId;
    public string? SharedSecret { get; set; } = sharedSecret;
    public string? Name { get; set; } = name;
    public string? Email { get; set; } = email;
    public string? Password { get; set; } = password;
    public DateTime LastUpdateAt { get; set; } = lastUpdateAt;
}
