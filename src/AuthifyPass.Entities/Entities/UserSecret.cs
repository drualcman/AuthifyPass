namespace AuthifyPass.Entities.Entities;
/// <summary>
/// Represents a user's shared secret for TOTP-based authentication.
/// </summary>
public class UserSecret(string? clientId, string? userId, string? activeSharedSecret)
{
    /// <summary>
    /// Identifier for the client (e.g., application)
    /// </summary>
    public string? ClientId => clientId;
    /// <summary>
    /// Identifier for the user
    /// </summary>
    public string? UserId => userId;
    public string? ActiveSharedSecret => activeSharedSecret;
}

