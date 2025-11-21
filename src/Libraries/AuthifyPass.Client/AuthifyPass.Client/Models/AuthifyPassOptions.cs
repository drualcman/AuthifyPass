namespace AuthifyPass.Client.Models;

public class AuthifyPassOptions
{
    public static string SectionKey => nameof(AuthifyPassOptions);

    public string ClientID { get; set; }
    public string Secret { get; set; }
    public string BaseUrl { get; set; }
    public string UserEndpoint { get; set; }
    public string ValidateCodeEndpoint { get; set; }
    public string Header { get; set; } = "x-authify-key";
}
