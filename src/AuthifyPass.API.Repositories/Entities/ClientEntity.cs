namespace AuthifyPass.API.Repositories.Entities;
/// <summary>
/// Represents a client integrating the authenticator API.
/// </summary>
public class ClientEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string ClientId { get; set; }
    public string SharedSecret { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastUpdateAt { get; set; }
}
