namespace AuthifyPass.Views.ViewModel;
internal class Add2FAViewModel : IAdd2FAViewModel<TwoFactorCode>
{
    readonly IRepository Repository;
    public IModelValidatorHub<IAdd2FAViewModel<TwoFactorCode>> Validator { get; private set; }

    public Add2FAViewModel(IRepository repository, ICameraService<TwoFactorCode> cameraService,
        IModelValidatorHub<IAdd2FAViewModel<TwoFactorCode>> validator)
    {
        Repository = repository;
        Validator = validator;
        cameraService.OnCapture += CameraService_OnCapture;
    }

    private async Task CameraService_OnCapture(TwoFactorCode data)
    {
        string title = Title;
        ClientId = data.ClientId;
        Name = data.Name;
        SharedKey = data.SharedKey;
        CreatedAt = DateTime.Now;
        if (await Validator.Validate(this))
            await Repository.AddTwoFactorCode(ToDto());
    }

    public string Title { get; set; }
    public string Name { get; set; }
    public string ClientId { get; set; }
    public string SharedKey { get; set; }
    public DateTime CreatedAt { get; set; }

    public TwoFactorCode ToDto() =>
        new TwoFactorCode
        {
            Title = this.Title,
            Name = this.Name,
            ClientId = this.ClientId,
            SharedKey = this.SharedKey,
            CreatedAt = this.CreatedAt,
            CurrentCode = string.Empty
        };

}
