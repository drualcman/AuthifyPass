namespace AuthifyPass.Client.Core.Interfaces;
public interface ICameraService
{
    event Func<string, Task> OnCapture;
    Task Capture(string data);
}
