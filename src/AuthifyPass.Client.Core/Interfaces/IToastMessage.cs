namespace AuthifyPass.Client.Core.Interfaces;
public interface IToastMessage
{
    Task Information(string message, int timer = 3);
    Task Success(string message, int timer = 3);
    Task Error(string message, int timer = 9);
    Task Warning(string message, int timer = 6);
    event Func<string, int, Task> OnInformation;
    event Func<string, int, Task> OnSuccess;
    event Func<string, int, Task> OnWarning;
    event Func<string, int, Task> OnError;
}
