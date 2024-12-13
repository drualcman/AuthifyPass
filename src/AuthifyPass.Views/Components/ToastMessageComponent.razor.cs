using System.Collections.Concurrent;

namespace AuthifyPass.Views.Components;
public partial class ToastMessageComponent : IDisposable
{
    private class IncomeMessage(string message, Level level)
    {
        public string Message => message;
        public Level Level => level;
    }

    private enum Level
    {
        Information, Success, Warning, Error
    }

    [Inject] IToastMessage ToastMessage { get; set; }
    ConcurrentDictionary<Guid, IncomeMessage> Messages = [];

    protected override void OnInitialized()
    {
        ToastMessage.OnInformation += ToastMessage_OnInformation;
        ToastMessage.OnError += ToastMessage_OnError;
        ToastMessage.OnSuccess += ToastMessage_OnSuccess;
        ToastMessage.OnWarning += ToastMessage_OnWarning;
    }

    private async Task ToastMessage_OnInformation(string message, int timer) => await AddMessageToQueue(new(message, Level.Information), timer);

    private async Task ToastMessage_OnWarning(string message, int timer) => await AddMessageToQueue(new(message, Level.Warning), timer);

    private async Task ToastMessage_OnSuccess(string message, int timer) => await AddMessageToQueue(new(message, Level.Success), timer);

    private async Task ToastMessage_OnError(string message, int timer) => await AddMessageToQueue(new(message, Level.Error), timer);

    private async Task AddMessageToQueue(IncomeMessage message, int timer)
    {
        _ = Task.Run(async () =>
        {
            Guid messageId = Guid.NewGuid();
            try
            {
                Messages[messageId] = message;
                await InvokeAsync(StateHasChanged);
                int waitingSeconds = timer * 1000;
                await Task.Delay(waitingSeconds);
                RemoveItem(messageId);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteAsync($"Error while removing message: {ex.Message}");
            }
            finally
            {
                await InvokeAsync(StateHasChanged);
            }
        });
        await InvokeAsync(StateHasChanged);
    }

    void RemoveItem(Guid messageId)
    {
        if (!Messages.TryRemove(messageId, out _))
        {
            Messages.Clear();
        }
    }

    string GetCssClassFromLevel(Level level) => level switch
    {
        Level.Information => "is-info",
        Level.Success => "is-success",
        Level.Error => "is-danger",
        _ => "is-warning"
    };

    string GetIconClassFromLevel(Level level) => level switch
    {
        Level.Information => "fa-check-info",
        Level.Success => "fa-check-circle",
        _ => "fa-exclamation-circle"
    };

    public void Dispose()
    {
        ToastMessage.OnInformation -= ToastMessage_OnInformation;
        ToastMessage.OnError -= ToastMessage_OnError;
        ToastMessage.OnSuccess -= ToastMessage_OnSuccess;
        ToastMessage.OnWarning -= ToastMessage_OnWarning;
    }
}