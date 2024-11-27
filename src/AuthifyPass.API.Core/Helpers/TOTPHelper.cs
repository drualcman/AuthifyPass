namespace AuthifyPass.API.Core.Helpers;
public class TOTPHelper
{
    public static bool ValidateTOTP(string code, string sharedSecret)
    {
        string generatedCode = TOTPGeneratorHelper.GenerateTOTP(sharedSecret);
        return generatedCode == code;
    }
}
