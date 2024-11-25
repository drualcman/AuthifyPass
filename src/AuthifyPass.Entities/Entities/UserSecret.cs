namespace AuthifyPass.Entities.Entities;
/// <summary>
/// Represents a user's shared secret for TOTP-based authentication.
/// </summary>
public class UserSecret
{
    /// <summary>
    /// Identifier for the client (e.g., application)
    /// </summary>
    public string? ClientId { get; set; }  
    /// <summary>
    /// Identifier for the user
    /// </summary>
    public string? UserId { get; set; }
    public string? ActiveSharedSecret { get; set; } 
}

