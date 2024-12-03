namespace AuthifyPass.Views.Services;
internal class CameraService : ICameraService
{
    Lazy<Task<IJSObjectReference>> ModuleTask;

    public CameraService(IJSRuntime jsRuntime)
    {
        ModuleTask = new Lazy<Task<IJSObjectReference>>(() => GetJSObjectReference(jsRuntime));
    }

    private Task<IJSObjectReference> GetJSObjectReference(IJSRuntime jsRuntime) =>
    jsRuntime.InvokeAsync<IJSObjectReference>(
        "import", $"./_content/AuthifyPass.Views/js/cameraService.js?v={DateTime.Today.Year}{DateTime.Today.Month}{DateTime.Today.Day}").AsTask();

    public async Task StartCamera(string videoElementId)
    {
        IJSObjectReference jsReference = await ModuleTask.Value;
        await jsReference.InvokeVoidAsync("startCamera", videoElementId);
    }

    public async Task StopCamera()
    {
        IJSObjectReference jsReference = await ModuleTask.Value;
        await jsReference.InvokeVoidAsync("stopCamera");
    }

    public async Task<string> CaptureFrame(string videoElementId, string canvasElementId)
    {
        try
        {
            IJSObjectReference jsReference = await ModuleTask.Value;
            string result = await jsReference.InvokeAsync<string>("captureFrame", videoElementId, canvasElementId);
            Console.WriteLine("Captured Frame (Base64): " + result);
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error in CaptureFrame: " + ex.Message);
            throw;
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (ModuleTask.IsValueCreated)
        {
            try
            {
                IJSObjectReference jsReference = await ModuleTask.Value;
                await jsReference.DisposeAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
