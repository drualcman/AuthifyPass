using AuthifyPass.Client.Core.Interfaces;
using AuthifyPass.Client.Core.Models;

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

    private bool isDeleteModalVisible;

    private void OpenDeleteModal(TwoFactorCode code)
    {
        SelectedItem = code;
        isDeleteModalVisible = true;
    }

    private void CloseModal()
    {
        isDeleteModalVisible = false;
    }

    private void ClearSelectedCode()
    {
        SelectedItem = null;
    }

    private async Task DeleteSelectedCode()
    {
        if (SelectedItem is not null)
        {
            await Repository.Delete(SelectedItem.Id);
            await SetCodes();
        }
        CloseModal();
        ClearSelectedCode();
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
            Console.WriteLine($"antes {item.CurrentCode}");
            item.CurrentCode = TOTPGeneratorHelper.GenerateTOTP(item.SharedKey);
            Console.WriteLine($"despues {item.CurrentCode}");
        }
        await InvokeAsync(StateHasChanged);
    }
}