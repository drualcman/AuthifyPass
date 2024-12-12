namespace AuthifyPass.Views.Components;
public partial class CountDownComponent : IDisposable
{
    [Parameter, EditorRequired] public int TimeToCount { get; set; }
    [Parameter] public EventCallback OnReset { get; set; }

    private Timer? Timer;
    private int TimeLeft = 0;
    private int TimeCountDown = 30;
    protected override void OnInitialized()
    {
        SyncWithCurrentSecond();
        Timer = new Timer(UpdateTimer, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
    }

    protected override void OnParametersSet()
    {
        if (TimeCountDown != TimeToCount)
        {
            TimeCountDown = TimeToCount;
            TimeLeft = TimeCountDown;
        }
    }

    private void SyncWithCurrentSecond()
    {
        int currentSecond = DateTime.UtcNow.Second;
        int modCycle = currentSecond % TimeCountDown; // Determina en qué parte del ciclo estamos.
        TimeLeft = TimeCountDown - modCycle; // Calcula los segundos restantes en el ciclo actual.
    }

    private void UpdateTimer(object? state)
    {
        TimeLeft--;
        if (TimeLeft <= 0)
        {
            SyncWithCurrentSecond();
            if (OnReset.HasDelegate)
                InvokeAsync(async () => await OnReset.InvokeAsync());
        }
        InvokeAsync(StateHasChanged);
    }

    public void Dispose()
    {
        Timer?.Dispose();
    }
}