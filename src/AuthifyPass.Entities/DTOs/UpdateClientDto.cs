namespace AuthifyPass.Entities.DTOs;

public class UpdateClientDto
{
    public string? ClientId { get; set; }
    public string? SharedSecret { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
}
