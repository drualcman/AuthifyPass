using AuthifyPass.Views.Components;

namespace AuthifyPass.Views.Pages;
public partial class Add2FA : IDisposable
{
    [Inject] IAdd2FAViewModel<TwoFactorCode> ViewModel { get; set; }
    [Inject] ICameraService<TwoFactorCode> CameraService { get; set; }
    [Inject] NavigationManager NavigationManager { get; set; }

    CaptureQRComponent<TwoFactorCode> Reader;

    protected override void OnInitialized()
    {
        CameraService.OnCapture += CameraService_OnCapture;
    }

    private async Task CameraService_OnCapture(TwoFactorCode data)
    {
        await AddCode();
    }

    private async Task AddCode()
    {
        if (await ViewModel.AddCode())
            NavigationManager.NavigateTo("");
        else if (Reader is not null)
            Reader.IsShowContent = true;
    }

    public void Dispose()
    {
        CameraService.OnCapture -= CameraService_OnCapture;
    }
}