using ZXingBlazor.Components;

namespace AuthifyPass.Views.Components;
public partial class CaptureQRComponent<TDataModel>
{
    [Inject] ICameraService<TDataModel> CameraService { get; set; }
    [Parameter] public RenderFragment ChildContent { get; set; }

    bool IsShowContent = false;

    public async Task TryDecodeData(string decodedData)
    {
        BarCode = decodedData;
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


    public string? BarCode { get; set; }
    BarcodeReader barcodeReaderCustom;
    BarCodes barCodes;
    private ZXingOptions options = new ZXingOptions()
    {
        TimeBetweenDecodingAttempts = 5,
        Debug = true,
        DecodeAllFormats = true,
        Decodeonce = true,
        ShowSelectFile = true
    };

    private async Task DecodeFromImage()
    {
        BarCode = "";
        await barCodes!.DecodeFromImage();
        IsShowContent = true;
    }
}