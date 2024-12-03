namespace AuthifyPass.Views.Pages;
public partial class Add2FA
{
    [Inject] IRepository Repository { get; set; }
    [Inject] NavigationManager NavigationManager { get; set; }


    private TwoFactorCode newCode = new();
    private async Task AddCode()
    {
        newCode.CreatedAt = DateTime.UtcNow;
        await Repository.AddTwoFactorCode(newCode);
        NavigationManager.NavigateTo("");
    }

    private void SetData(TwoFactorCode data)
    {
        newCode = data;
    }
}