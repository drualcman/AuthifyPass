namespace AuthifyPass.Entities.Interfaces;
public interface IIdentifierGenerator
{
    string GenerateClientId();
    string GenerateSharedSecret();
    string ComputeSha256Hash(string input);
}
