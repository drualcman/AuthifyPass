namespace AuthifyPass.Entities.Helpers;
public class TOTPGeneratorHelper
{
    private const int TimeStep = 30;    // TOTP uses a time step of 30 seconds by default
    private const int Digits = 6;       // The length of the OTP (6 digits)

    // Method to generate the TOTP code
    public static string GenerateTOTP(string sharedSecret)
    {
        if (string.IsNullOrEmpty(sharedSecret))
        {
            throw new ArgumentException("Shared secret cannot be null or empty.");
        }

        // Convert the shared secret (hexadecimal string) to a byte array
        byte[] key = HexStringToByteArray(sharedSecret);

        // Get the current Unix time and force it into a consistent 30-second interval
        long currentUnixTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        long timeStep = (currentUnixTime / TimeStep) * TimeStep;

        // Convert time step to byte array (big-endian)
        byte[] timeBytes = BitConverter.GetBytes(timeStep);
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(timeBytes); // Ensure big-endian byte order
        }

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

    private static byte[] HexStringToByteArray(string hex)
    {
        if (hex.Length % 2 != 0)
        {
            throw new ArgumentException("Hexadecimal string must have an even length.");
        }

        byte[] bytes = new byte[hex.Length / 2];
        for (int i = 0; i < hex.Length; i += 2)
        {
            bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
        }
        return bytes;
    }
}
