namespace AuthifyPass.Views.ViewModel;
internal class Add2FAViewModel
{
    readonly IRepository Repository;
    public readonly IModelValidatorHub<Add2FAViewModel> Validator;

    public Add2FAViewModel(IRepository repository, ICameraService<TwoFactorCode> cameraService,
        IModelValidatorHub<Add2FAViewModel> validator)
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
            await Repository.AddTwoFactorCode((TwoFactorCode)this);
    }

    public int Id { get; set; }
    public string Title { get; set; }
    public string Name { get; set; }
    public string ClientId { get; set; }
    public string SharedKey { get; set; }
    public DateTime CreatedAt { get; set; }

    public static explicit operator TwoFactorCode(Add2FAViewModel model) =>
        new TwoFactorCode
        {
            Id = model.Id,
            Title = model.Title,
            Name = model.Name,
            ClientId = model.ClientId,
            SharedKey = model.SharedKey,
            CreatedAt = model.CreatedAt,
            CurrentCode = string.Empty
        };
}
