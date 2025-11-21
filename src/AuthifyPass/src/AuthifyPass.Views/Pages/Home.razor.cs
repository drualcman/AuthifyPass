namespace AuthifyPass.Views.Pages;
public partial class Home
{
    [Inject] IHomeViewModel ViewModel { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await ViewModel.GetCodes();
            await InvokeAsync(StateHasChanged);
        }
    }
}