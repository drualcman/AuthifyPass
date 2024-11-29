namespace AuthifyPass.Client.Core.Interfaces;
public interface ICameraService : IAsyncDisposable
{
    Task StartCamera(string videoElementId);
    Task StopCamera();
    Task<string> CaptureFrame(string videoElementId, string canvasElementId);
}
