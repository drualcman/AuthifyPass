namespace AuthifyPass.Client.Models;

public class AuthifyPassResponse(string sharedkey, string imageSVG)
{
    public string Sharedkey => sharedkey;
    public string ImageSVG => imageSVG;
}
