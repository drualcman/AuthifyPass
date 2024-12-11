using ZXingBlazor.Components;

namespace AuthifyPass.Views.Components;
public partial class CaptureQRComponent<TDataModel>
{
    [Inject] ICameraService<TDataModel> CameraService { get; set; }
    [Parameter] public RenderFragment HeaderContent { get; set; }
    [Parameter] public RenderFragment ChildContent { get; set; }
    [Parameter] public RenderFragment Buttons { get; set; }

    public bool IsShowContent { get; set; } = false;

    public async Task TryDecodeData(string decodedData)
    {
        TDataModel dataModel = default;
        try
        {
            dataModel = JsonSerializer.Deserialize<TDataModel>(decodedData);
        }
        catch (Exception ex)
        {
            await Console.Out.WriteLineAsync(ex.Message);
        }
        await CameraService.Capture(dataModel);
        await InvokeAsync(StateHasChanged);
    }

    void ShowContent()
    {
        IsShowContent = true;
    }

    void HideContent()
    {
        IsShowContent = false;
    }

    private async Task OnError(string message)
    {
        await Console.Out.WriteLineAsync(message);
        await InvokeAsync(StateHasChanged);
    }

    BarcodeReader BarcodeReaderCustom;
    BarCodes BarCodes;
    private ZXingOptions BarcodeOptions = new ZXingOptions()
    {
        TimeBetweenDecodingAttempts = 5,
        Debug = false,
        DecodeAllFormats = true,
        Decodeonce = true,
        ShowSelectFile = true
    };

    private async Task DecodeFromImage()
    {
        await BarCodes!.DecodeFromImage();
        IsShowContent = true;
    }
}