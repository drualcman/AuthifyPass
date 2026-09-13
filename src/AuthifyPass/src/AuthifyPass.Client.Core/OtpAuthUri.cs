using AuthifyPass.Client.Core.Models;

namespace AuthifyPass.Client.Core;

public static class OtpAuthUri
{
    public static string Build(TwoFactorCode code)
    {
        string issuer = string.IsNullOrWhiteSpace(code.Name) ? "AuthifyPass" : code.Name;
        string account = string.IsNullOrWhiteSpace(code.UserID) ? code.Description : code.UserID;
        string label = $"{Uri.EscapeDataString(issuer)}:{Uri.EscapeDataString(account)}";

        string result = $"otpauth://totp/{label}"
            + $"?secret={code.SharedKey}"
            + $"&issuer={Uri.EscapeDataString(issuer)}"
            + "&algorithm=SHA1"
            + $"&digits={code.Digits}"
            + $"&period={code.Period}";

        return result;
    }
}
