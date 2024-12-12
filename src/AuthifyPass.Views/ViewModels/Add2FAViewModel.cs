namespace AuthifyPass.Views.ViewModels;
internal class Add2FAViewModel : IAdd2FAViewModel<TwoFactorCode>, IDisposable
{
    readonly IRepository Repository;
    readonly ICameraService<TwoFactorCode> CameraService;
    public IModelValidatorHub<IAdd2FAViewModel<TwoFactorCode>> Validator { get; private set; }

    public Add2FAViewModel(IRepository repository, ICameraService<TwoFactorCode> cameraService,
        IModelValidatorHub<IAdd2FAViewModel<TwoFactorCode>> validator)
    {
        CameraService = cameraService;
        Repository = repository;
        Validator = validator;
        CameraService.OnCapture += CameraService_OnCapture;
    }

    private Task CameraService_OnCapture(TwoFactorCode data)
    {
        ClientId = data.ClientId;
        Name = data.Name;
        SharedKey = data.SharedKey;
        CreatedAt = DateTime.Now;
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
    public string ClientId { get; set; }
    public string SharedKey { get; set; }
    public DateTime CreatedAt { get; set; }

    public TwoFactorCode ToDto() =>
        new TwoFactorCode
        {
            Description = this.Description,
            Name = this.Name,
            ClientId = this.ClientId,
            SharedKey = this.SharedKey,
            CreatedAt = this.CreatedAt,
            CurrentCode = string.Empty
        };

    public void Dispose()
    {
        CameraService.OnCapture -= CameraService_OnCapture;
    }
}
