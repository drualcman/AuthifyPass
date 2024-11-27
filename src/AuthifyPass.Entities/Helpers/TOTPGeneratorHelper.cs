namespace AuthifyPass.Entities.Helpers;
public class TOTPGeneratorHelper
{
    private const int TimeStep = 30;    // TOTP uses a time step of 30 seconds by default
    private const int Digits = 6;       // The length of the OTP (6 digits)

    // Method to generate the TOTP code
    public static string GenerateTOTP(string sharedSecret)
    {
        // Convert the shared secret to a byte array
        byte[] key = Base32Decode(sharedSecret);

        // Get the current Unix time
        long timeStep = DateTimeOffset.UtcNow.ToUnixTimeSeconds() / TimeStep;

        // Convert time step to byte array (big-endian)
        byte[] timeBytes = BitConverter.GetBytes(timeStep);
        Array.Reverse(timeBytes); // Ensure big-endian byte order

        // Generate HMAC-SHA1 hash
        using var hmac = new HMACSHA1(key);
        byte[] hash = hmac.ComputeHash(timeBytes);
        int offset = hash[hash.Length - 1] & 0x0F;
        int binaryCode = (hash[offset] & 0x7F) << 24 |
                         (hash[offset + 1] & 0xFF) << 16 |
                         (hash[offset + 2] & 0xFF) << 8 |
                         hash[offset + 3] & 0xFF;

        return BinaryCodeToOTP(binaryCode);
    }

    // Generate OTP (modulo 10^6 to ensure it's 6 digits)
    private static string BinaryCodeToOTP(int binaryCode) => (binaryCode % (int)Math.Pow(10, Digits)).ToString($"D{Digits}");

    private static byte[] Base32Decode(string base32)
    {
        const string base32Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
        StringBuilder sb = new StringBuilder();
        foreach (char c in base32)
        {
            sb.Append(base32Alphabet.IndexOf(char.ToUpper(c), StringComparison.Ordinal));
        }

        byte[] result = new byte[sb.Length * 5 / 8];
        int index = 0;
        for (int i = 0; i < sb.Length; i += 8)
        {
            long value = 0;
            for (int j = 0; j < 8 && i + j < sb.Length; j++)
            {
                value = value << 5 | sb[i + j];
            }

            for (int j = 0; j < 5; j++)
            {
                result[index++] = (byte)(value >> 32 - 8 * (j + 1) & 0xFF);
            }
        }

        return result;
    }
}
