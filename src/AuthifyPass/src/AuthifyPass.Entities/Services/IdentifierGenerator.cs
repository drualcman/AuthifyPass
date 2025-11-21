namespace AuthifyPass.Entities.Services
{
    internal class IdentifierGenerator : IIdentifierGenerator
    {
        public string GenerateClientId()
        {
            return Guid.NewGuid().ToString("N");
        }

        public string GenerateSharedSecret()
        {
            byte[] keyBytes = RandomNumberGenerator.GetBytes(20); // 160-bit key (RFC 4226/TOTP standard)
            string base32Key = Base32Encode(keyBytes);
            return base32Key;
        }

        public string ComputeSha256Hash(string input)
        {
            using SHA256 sha256 = SHA256.Create();
            byte[] data = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sb = new StringBuilder(data.Length * 2);

            int index = 0;
            while (index < data.Length)
            {
                sb.Append(data[index].ToString("x2"));
                index++;
            }

            return sb.ToString();
        }

        private string Base32Encode(byte[] data)
        {
            const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
            StringBuilder result = new StringBuilder((data.Length + 4) / 5 * 8);

            int buffer = 0;
            int bitsLeft = 0;
            int index = 0;

            while (index < data.Length)
            {
                buffer = (buffer << 8) | data[index];
                bitsLeft += 8;
                index++;

                while (bitsLeft >= 5)
                {
                    int value = (buffer >> (bitsLeft - 5)) & 0x1F;
                    bitsLeft -= 5;
                    result.Append(alphabet[value]);
                }
            }

            if (bitsLeft > 0)
            {
                int value = (buffer << (5 - bitsLeft)) & 0x1F;
                result.Append(alphabet[value]);
            }

            return result.ToString();
        }
    }
}
