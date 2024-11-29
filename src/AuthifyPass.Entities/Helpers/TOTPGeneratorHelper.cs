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

        // Convert the shared secret to a byte array
        byte[] key = Base32Decode(sharedSecret);

        // Get the current Unix time
        long timeStep = DateTimeOffset.UtcNow.ToUnixTimeSeconds() / TimeStep;

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

    private static byte[] Base32Decode(string base32)
    {
        const string base32Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
        if (string.IsNullOrEmpty(base32))
        {
            throw new ArgumentException("Invalid base32 input: input cannot be null or empty.");
        }

        // Normalize the input
        base32 = base32.TrimEnd('=').ToUpperInvariant();

        int byteCount = base32.Length * 5 / 8; // Base32 encodes 5 bits per character
        byte[] result = new byte[byteCount];

        int bitBuffer = 0;
        int bitsLeft = 0;
        int resultIndex = 0;

        foreach (char c in base32)
        {
            int charIndex = base32Alphabet.IndexOf(c);
            if (charIndex < 0)
            {
                throw new ArgumentException($"Invalid base32 character: {c}");
            }

            bitBuffer = (bitBuffer << 5) | charIndex;
            bitsLeft += 5;

            if (bitsLeft >= 8)
            {
                result[resultIndex++] = (byte)(bitBuffer >> (bitsLeft - 8));
                bitsLeft -= 8;
            }
        }

        return result;
    }
}
