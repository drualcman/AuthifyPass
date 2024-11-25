namespace AuthifyPass.API.Repositories.Entities;
/// <summary>
/// Represents a user's shared secret for TOTP-based authentication.
/// </summary>
public class UserSecretEntity
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
    public string? PreviousSharedSecret { get; set; }
    public DateTime CreatetAt { get; set; } = DateTime.UtcNow;
    public DateTime? PreviousSecretExpiryDate { get; set; }
    public DateTime LastRotatedAt { get; set; } = DateTime.UtcNow;
}

