namespace AuthifyPass.API.Components.Pages;
public partial class Register
{
    [Inject] IConfiguration configuration { get; set; }
    [Inject] IStringCulture<RegisterPageContent> Localizer { get; set; }
    [Inject] IRegisterClientController RegisterClient { get; set; }
    [Inject] IModelValidatorHub<RegisterClientDto> Validator { get; set; }

    RegisterClientModel Client = new();
    RegisterClientResponseDto Response;
    bool IsWorking;
    bool IsRegistered = false;
    string ErrorMessage = string.Empty;
    long TimeStep = 0;
    string SharedSecret;
    string GeneratedCode;
    MarkupString HelpString;
    bool IsValid = false;

    CaptchaComponent Captcha;

    CaptchaProperties CaptchaOptions;

    protected override void OnInitialized()
    {
        SharedSecret = configuration.GetValue<string>("Secret");
        TimeStep = TOTPGeneratorHelper.CalculateTimeStep();
        GeneratedCode = TOTPGeneratorHelper.GenerateTOTP(SharedSecret, TimeStep);
        CaptchaOptions = new CaptchaProperties(
            Type: CaptchaType.Custom,
            Placeholder: Localizer[nameof(RegisterPageContent.ValidationMessage)],
            ErrorMessage: Localizer[nameof(RegisterPageContent.ValidationCodeError)]);
    }

    Task<IEnumerable<CaptchaItem>> CaptchaSource()
    {
        List<CaptchaItem> items = [new CaptchaItem("\\/", GeneratedCode)];
        return Task.FromResult(items.AsEnumerable());
    }

    async Task SendRegister()
    {
        IsWorking = true;
        ClearError();
        try
        {
            Response = await RegisterClient.CreateClientAsync(new(Client.Name, Client.Email, Client.Password, GeneratedCode));
            string help = Localizer[nameof(RegisterPageContent.NotificationBodyHtmlTemplate)];
            HelpString = new(help.Replace("{0}", Response.SharedSecret).Replace("{1}", Response.ClientId));
            IsRegistered = true;
        }
        catch (Entities.Exceptions.ValidationException ex)
        {
            ErrorMessage = string.Join(". ", ex.Errors.Select(e => $"{e.PropertyName}: {e.Message}"));
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
        finally
        {
            IsWorking = false;
        }
    }
    void ClearError()
    {
        ErrorMessage = string.Empty;
    }

    async Task RefreshCode()
    {
        TimeStep = TOTPGeneratorHelper.CalculateTimeStep();
        GeneratedCode = TOTPGeneratorHelper.GenerateTOTP(SharedSecret, TimeStep);
        IsValid = false;
        await Captcha.Refresh();
    }

    private class RegisterClientModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}