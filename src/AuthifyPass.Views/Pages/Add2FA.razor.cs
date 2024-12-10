namespace AuthifyPass.Views.Pages;
public partial class Add2FA
{
    [Inject] IAdd2FAViewModel<TwoFactorCode> ViewModel { get; set; }
    [Inject] ICameraService<TwoFactorCode> CameraService { get; set; }
    [Inject] NavigationManager NavigationManager { get; set; }

    protected override void OnInitialized()
    {
        CameraService.OnCapture += CameraService_OnCapture;
    }

    private async Task CameraService_OnCapture(TwoFactorCode data)
    {
        await AddCode();
    }

    private Task AddCode()
    {
        NavigationManager.NavigateTo("");
        return Task.CompletedTask;
    }
}