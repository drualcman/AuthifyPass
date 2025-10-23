using OtpNet;

namespace AuthifyPass.Entities.Helpers;

public static class TOTPGeneratorHelper
{
    private const int DefaultTimeStep = 30; // 30-second interval by default.
    private const int DefaultDigits = 6;   // Standard TOTP uses 6-digit codes.

    public static string GenerateTOTP(string sharedSecret, int timeStepSeconds = DefaultTimeStep, int digits = DefaultDigits)
    {
        if (string.IsNullOrWhiteSpace(sharedSecret))
        {
            throw new ArgumentException("Shared secret cannot be null or empty.");
        }

        byte[] secretKey = GetSecretBytes(sharedSecret);

        // Create a TOTP generator using Otp.NET
        Totp totp = new Totp(secretKey, timeStepSeconds, OtpHashMode.Sha1, digits);

        string totpCode = totp.ComputeTotp();
        return totpCode;
    }

    public static bool ValidateTOTP(string code, string sharedSecret, int period = 30, int digits = 6)
    {
        //return false;
        byte[] secretKey = GetSecretBytes(sharedSecret);

        var totp = new Totp(secretKey, step: period, totpSize: digits);

        // Validate the code (includes a small tolerance for clock drift)
        bool isValid = totp.VerifyTotp(code, out long timeWindowUsed);
        return isValid;
    }

    private static byte[] GetSecretBytes(string sharedSecret)
    {
        // Si la clave parece ser Base32 (como "JBSWY3DPEHPK3PXP")
        if (IsBase32String(sharedSecret))
        {
            // Usar el decodificador Base32 de OtpNet
            string cleanSecret = sharedSecret.Replace(" ", "").ToUpper();
            return Base32Encoding.ToBytes(cleanSecret);
        }
        else
        {
            // Si no es Base32, asumir que es hexadecimal (tu código original)
            return HexStringToByteArray(sharedSecret);
        }
    }

    private static byte[] HexStringToByteArray(string hex)
    {
        int numberOfChars = hex.Length;
        byte[] secretKey = new byte[numberOfChars / 2];
        int index = 0;
        while (index < numberOfChars)
        {
            string byteValue = hex.Substring(index, 2);
            secretKey[index / 2] = Convert.ToByte(byteValue, 16);
            index = index + 2;
        }
        return secretKey;
    }

    private static bool IsBase32String(string input)
    {
        if (string.IsNullOrEmpty(input))
            return false;

        // Los caracteres válidos en Base32 son A-Z, 2-7, y opcionalmente = para padding
        string cleanInput = input.Replace(" ", "").ToUpper();

        foreach (char c in cleanInput)
        {
            if (!((c >= 'A' && c <= 'Z') || (c >= '2' && c <= '7') || c == '='))
            {
                return false;
            }
        }
        return true;
    }

    private static byte[] GetSecretBytes1(string sharedSecret)
    {
        // Convert hexadecimal string to bytes
        int numberOfChars = sharedSecret.Length;
        byte[] secretKey = new byte[numberOfChars / 2];
        int index = 0;
        while (index < numberOfChars)
        {
            string byteValue = sharedSecret.Substring(index, 2);
            secretKey[index / 2] = Convert.ToByte(byteValue, 16);
            index = index + 2;
        }

        return secretKey;
    }
}

