using Microsoft.AspNetCore.Components;

namespace AuthifyPass.Views.Components;
public partial class CountDownComponent : IDisposable
{
    [Parameter, EditorRequired] public int TimeToCount { get; set; }
    [Parameter] public EventCallback OnReset { get; set; }

    private Timer? Timer;
    private int TimeLeft = 0;
    private int TimeCountDown = 0;
    protected override void OnInitialized()
    {
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

    private void UpdateTimer(object? state)
    {
        TimeLeft = (TimeLeft - 1) % 30;
        if (TimeLeft == 0)
        {
            TimeLeft = TimeCountDown;
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