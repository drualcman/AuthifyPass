namespace AuthifyPass.Views.Pages;
public partial class Add2FA
{
    [Inject] IRepository Repository { get; set; }
    [Inject] ICameraService<TwoFactorCode> CameraService { get; set; }
    [Inject] NavigationManager NavigationManager { get; set; }

    protected override void OnInitialized()
    {
        CameraService.OnCapture += CameraService_OnCapture;
    }

    private async Task CameraService_OnCapture(TwoFactorCode data)
    {
        string title = newCode.Title;
        newCode = data;
        newCode.Title = title;
        await AddCode();
    }

    private TwoFactorCode newCode = new();

    private async Task AddCode()
    {
        newCode.CreatedAt = DateTime.UtcNow;
        await Repository.AddTwoFactorCode(newCode);
        NavigationManager.NavigateTo("");
    }
}