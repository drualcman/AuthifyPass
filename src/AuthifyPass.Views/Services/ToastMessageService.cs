namespace AuthifyPass.Views.Services;
internal class ToastMessageService : IToastMessage
{
    public event Func<string, int, Task> OnInformation;
    public event Func<string, int, Task> OnSuccess;
    public event Func<string, int, Task> OnWarning;
    public event Func<string, int, Task> OnError;

    public async Task Information(string message, int timer = 3)
    {
        if (OnInformation != null)
            await OnInformation(message, timer);
    }
    public async Task Error(string message, int timer = 9)
    {
        if (OnError != null)
            await OnError(message, timer);
    }

    public async Task Success(string message, int timer = 3)
    {
        if (OnSuccess != null)
            await OnSuccess(message, timer);
    }

    public async Task Warning(string message, int timer = 6)
    {
        if (OnWarning != null)
            await OnWarning(message, timer);
    }
}
