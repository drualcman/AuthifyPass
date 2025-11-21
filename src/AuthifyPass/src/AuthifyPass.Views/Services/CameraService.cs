
namespace AuthifyPass.Views.Services;
internal class CameraService : ICameraService
{
    public event Func<string, Task> OnCapture;

    public async Task Capture(string data)
    {
        if (OnCapture is not null)
            await OnCapture(data);
    }
}
