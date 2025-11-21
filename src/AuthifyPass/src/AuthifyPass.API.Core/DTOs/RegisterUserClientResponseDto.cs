namespace AuthifyPass.API.Core.DTOs;
public class RegisterUserClientResponseDto(string sharedkey, string imageSVG)
{
    public string Sharedkey => sharedkey;
    public string ImageSVG => imageSVG;
}
