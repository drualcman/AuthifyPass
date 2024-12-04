
namespace AuthifyPass.Views.Services;
internal class CameraService<TDataModel> : ICameraService<TDataModel> where TDataModel : class
{
    public event Func<TDataModel, Task> OnCapture;

    public async Task Capture(TDataModel data)
    {
        if (OnCapture is not null)
            await OnCapture(data);
    }
}
