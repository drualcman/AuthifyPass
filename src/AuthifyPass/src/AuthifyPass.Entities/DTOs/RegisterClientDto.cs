namespace AuthifyPass.Entities.DTOs;

public class RegisterClientDto(string name, string email, string password, string code)
{
    public string Name => name;
    public string Email => email;
    public string Password => password;
    public string Code => code;
}

