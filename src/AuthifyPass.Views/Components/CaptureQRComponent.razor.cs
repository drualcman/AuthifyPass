using AuthifyPass.Client.Core.Helper;
using BlazorBasics.InputFileExtended.Models;
using BlazorBasics.InputFileExtended.ValueObjects;

namespace AuthifyPass.Views.Components;
public partial class CaptureQRComponent<TDataModel> where TDataModel : class
{
    [Inject] ICameraService CameraService { get; set; }
    [Parameter] public RenderFragment ChildContent { get; set; }
    [Parameter] public EventCallback<TDataModel> OnReadBarcode { get; set; }

    bool IsShowContent = false;
    InputFileParameters InputFileParameters = new InputFileParameters
    {
        MaxUploatedFiles = 1,
        MultiFile = false,
        AllowPasteFiles = false,
        DragAndDropOptions = new DragAndDropOptions
        {
            CanDropFiles = true
        },
        PreviewOptions = new PreviewOptions
        {
            CanDeleteIfNotCallBack = true,
            IsImage = true,
            ShowPreview = true
        }
    };

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await CameraService.StartCamera("video");
        }
    }

    private async Task StartCamera()
    {
        await CameraService.StartCamera("video");
    }

    private async Task CaptureFrame()
    {
        string base64Image = await CameraService.CaptureFrame("video", "canvas");

        await Console.Out.WriteLineAsync("------ CAPTURED IMAGE --------");
        await Console.Out.WriteLineAsync(base64Image);
        await Console.Out.WriteLineAsync("--------------");
        await DecodeBase64Image(base64Image);
    }

    private async Task CaptureImage(FilesUploadEventArgs e)
    {
        await Console.Out.WriteLineAsync("------ UPLOAD IMAGE --------");
        EventAction action = e.Action;
        await Console.Out.WriteLineAsync($"------ {action.ToString()} --------");
        if (action == EventAction.Added)
        {
            await Console.Out.WriteLineAsync("------- DECODING -------");
            string image = e.Files[0].ToImageHTML;
            await Console.Out.WriteLineAsync(image);
            await DecodeBase64Image(image);
        }
        await Console.Out.WriteLineAsync("--------------");
    }

    private async Task DecodeBase64Image(string base64Image)
    {
        await Console.Out.WriteLineAsync("------ DECODING IMAGE --------");
        string decodedData = QRDecoder.Decode(base64Image);
        await Console.Out.WriteLineAsync(decodedData);
        await Console.Out.WriteLineAsync("--------------");
        await TryDecodeData(decodedData);
    }

    private async Task TryDecodeData(string decodedData)
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
        if (OnReadBarcode.HasDelegate)
            await OnReadBarcode.InvokeAsync(dataModel);
    }

    private async Task StopCamera()
    {
        await CameraService.StopCamera();
    }

    async Task ShowContent()
    {
        await CameraService.StopCamera();
        IsShowContent = true;
    }

    void HideContent()
    {
        IsShowContent = false;
        Task.Run(async () =>
        {
            await Task.Delay(100);
            await CameraService.StartCamera("video");
        });
    }
}