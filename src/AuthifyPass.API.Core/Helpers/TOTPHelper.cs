namespace AuthifyPass.API.Core.Helpers;
public class TOTPHelper
{
    public static bool ValidateTOTP(string code, string sharedSecret)
    {
        long timeStep = TOTPGeneratorHelper.CalculateTimeStep();
        string generatedCode = TOTPGeneratorHelper.GenerateTOTP(sharedSecret, timeStep);
        return generatedCode == code;
    }
}
