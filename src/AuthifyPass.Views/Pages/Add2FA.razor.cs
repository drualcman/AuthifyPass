using AuthifyPass.Client.Core.Helper;

namespace AuthifyPass.Views.Pages;
public partial class Add2FA
{
    [Inject] ICameraService CameraService { get; set; }
    [Inject] IRepository Repository { get; set; }
    [Inject] IJSRuntime JSRuntime { get; set; }
    [Inject] NavigationManager NavigationManager { get; set; }

    private TwoFactorCode newCode = new();

    private async Task AddCode()
    {
        newCode.CreatedAt = DateTime.UtcNow;
        await Repository.AddTwoFactorCode(newCode);
        NavigationManager.NavigateTo("");
    }

    private string DecodedData = string.Empty;

    private async Task StartCamera()
    {
        await CameraService.StartCamera("video");
    }

    private async Task CaptureFrame()
    {
        string base64Image = await CameraService.CaptureFrame("video", "canvas");
        DecodedData = QRDecoder.Decode(base64Image);
    }

    private async Task StopCamera()
    {
        await CameraService.StopCamera();
    }
}