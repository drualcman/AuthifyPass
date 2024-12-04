namespace AuthifyPass.Client.Core.Interfaces;
public interface ICameraService<TModel>
{
    event Func<TModel, Task> OnCapture;
    Task Capture(TModel data);
}
