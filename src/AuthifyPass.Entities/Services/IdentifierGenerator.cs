namespace AuthifyPass.Entities.Services;
internal class IdentifierGenerator : IIdentifierGenerator
{
    public string GenerateClientId()
    {
        return Guid.NewGuid().ToString("N");
    }

    public string GenerateSharedSecret()
    {
        string input = Guid.NewGuid().ToString();
        return ComputeSha256Hash(input);
    }

    public string ComputeSha256Hash(string input)
    {
        using SHA256 sha256 = SHA256.Create();
        byte[] data = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));

        StringBuilder sb = new StringBuilder();
        foreach (byte b in data)
        {
            sb.Append(b.ToString("x2"));
        }
        return sb.ToString();
    }
}
