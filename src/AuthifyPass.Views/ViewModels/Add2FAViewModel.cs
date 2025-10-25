namespace AuthifyPass.Views.ViewModels;
internal class Add2FAViewModel : IAdd2FAViewModel<TwoFactorCode>, IDisposable
{
    readonly IRepository Repository;
    readonly ICameraService CameraService;
    readonly IStringLocalizer<Add2FAPageContent> Content;
    public IModelValidatorHub<IAdd2FAViewModel<TwoFactorCode>> Validator { get; private set; }

    public Add2FAViewModel(IRepository repository, ICameraService cameraService,
        IModelValidatorHub<IAdd2FAViewModel<TwoFactorCode>> validator,
        IStringLocalizer<Add2FAPageContent> content)
    {
        CameraService = cameraService;
        Repository = repository;
        Validator = validator;
        Content = content;
        CameraService.OnCapture += CameraService_OnCapture;
    }

    public string TitleText => Content[nameof(Add2FAPageContent.TitleText)];
    public string DescriptionText => Content[nameof(Add2FAPageContent.DescriptionLabelText)];
    public string AddButtonText => Content[nameof(Add2FAPageContent.AddButtonText)];
    public string CancelButtonText => Content[nameof(Add2FAPageContent.CancelButtonText)];
    public string FromImageButtonText => Content[nameof(Add2FAPageContent.FromImageButtonText)];
    public string ManualButtonText => Content[nameof(Add2FAPageContent.ManualButtonText)];
    public string CompanyText => Content[nameof(Add2FAPageContent.CompanyText)];
    public string CodeText => Content[nameof(Add2FAPageContent.CodeText)];
    public string SecretText => Content[nameof(Add2FAPageContent.SecretText)];

    private Task CameraService_OnCapture(string otpauthUrl)
    {
        var uri = new Uri(otpauthUrl);
        if (uri.Scheme != "otpauth")
            throw new FormatException("Invalid OTP Auth scheme.");

        string[] pathSegments = uri.AbsolutePath.Split(':');
        string userId = pathSegments.Length > 1 ? pathSegments[0].Trim('/') : string.Empty;
        string name = System.Web.HttpUtility.UrlDecode(pathSegments[1].Trim('/'));
        var queryParams = System.Web.HttpUtility.ParseQueryString(uri.Query);

        Description = userId.Contains('@') ? $"{uri.Scheme}://{name}" : name;
        Name = queryParams["issuer"] ?? name;
        UserId = userId;
        SharedKey = queryParams["secret"] ?? string.Empty;
        CreatedAt = DateTime.Now;
        Period = int.TryParse(queryParams["period"], out int period) ? period : 30;
        Digits = int.TryParse(queryParams["digits"], out int digits) ? digits : 6;
        return Task.CompletedTask;
    }

    public async Task<bool> AddCode()
    {
        bool result = await Validator.Validate(this);
        if (result)
            await Repository.AddTwoFactorCode(ToDto());
        return result;
    }

    public string Description { get; set; }
    public string Name { get; set; }
    public string UserId { get; set; }
    public string SharedKey { get; set; }
    DateTime CreatedAt;
    int Period;
    int Digits;

    public TwoFactorCode ToDto() =>
        new TwoFactorCode
        {
            Description = this.Description,
            Name = this.Name,
            UserID = this.UserId,
            SharedKey = this.SharedKey,
            CreatedAt = this.CreatedAt,
            CurrentCode = string.Empty,
            Period = this.Period,
            Digits = this.Digits
        };

    public void Dispose()
    {
        CameraService.OnCapture -= CameraService_OnCapture;
    }
}
