namespace AuthifyPass.Entities.Interfaces;
public interface IIdentifierGenerator
{
    string GenerateClientId();
    string GenerateSharedSecret();
}
