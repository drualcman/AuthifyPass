namespace AuthifyPass.Views.Pages;
public partial class Home
{
    [Inject] IRepository Repository { get; set; }
    [Inject] IJSRuntime JSRuntime { get; set; }

    private List<TwoFactorCode> TwoFactorCodes = [];
    private TwoFactorCode? SelectedItem = null;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await SetCodes();
        }
    }

    private async Task CopyToClipboard(string code)
    {
        await JSRuntime.InvokeVoidAsync("navigator.clipboard.writeText", code);
    }

    private bool IsDeleteModalVisible;
    private bool IsDeleteDeleting;

    private void OpenDeleteModal(TwoFactorCode code)
    {
        SelectedItem = code;
        IsDeleteModalVisible = true;
    }

    private void CloseModal()
    {
        IsDeleteModalVisible = false;
    }

    private void ClearSelectedCode()
    {
        SelectedItem = null;
    }

    private async Task DeleteSelectedCode()
    {
        if (SelectedItem is not null)
        {
            IsDeleteDeleting = true;
            await Repository.Delete(SelectedItem.Id);
            await SetCodes();
        }
        CloseModal();
        ClearSelectedCode();
        IsDeleteDeleting = false;
    }

    async Task SetCodes()
    {
        TwoFactorCodes = new(await Repository.GetTwoFactorCodes());
        await RefreshCodes();

    }

    private async Task RefreshCodes()
    {
        foreach (var item in TwoFactorCodes)
        {
            item.CurrentCode = TOTPGeneratorHelper.GenerateTOTP(item.SharedKey);
        }
        await InvokeAsync(StateHasChanged);
    }
}