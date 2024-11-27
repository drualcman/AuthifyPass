namespace AuthifyPass.Entities.DTOs;

public class RegisterClientDto(string name, string email, string password)
{
    public string Name => name;
    public string Email => email;
    public string Password => password;
}

