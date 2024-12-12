namespace AuthifyPass.Views.ViewModels;
internal class HomeViewModel(
    IRepository Repository,
    IJSRuntime JSRuntime,
    IStringLocalizer<HomePageContent> content) : IHomeViewModel
{

    private List<TwoFactorCode> TwoFactorCodesBK = [];
    private IEnumerable<TwoFactorCode> TwoFactorCodesFiltered = [];
    private string SearchTextBK = "";
    public IEnumerable<TwoFactorCode> TwoFactorCodes => TwoFactorCodesFiltered;
    public TwoFactorCode? SelectedItem { get; private set; }
    public string SearchText
    {
        get => SearchTextBK;
        set
        {
            SearchTextBK = value;
            ExecuteSearch();
        }
    }

    private void ExecuteSearch()
    {
        if (string.IsNullOrEmpty(SearchTextBK))
        {
            TwoFactorCodesFiltered = TwoFactorCodesBK.ToList();
        }
        else
        {
            TwoFactorCodesFiltered = TwoFactorCodesBK
                .Where(n =>
                    n.Name.Contains(SearchTextBK, StringComparison.InvariantCultureIgnoreCase) ||
                    n.Description.Contains(SearchTextBK, StringComparison.InvariantCultureIgnoreCase))
                .ToList();
        }
    }

    public bool IsModalVisible { get; set; }
    public bool IsDeleting { get; private set; }
    public bool HasCodes => TwoFactorCodesBK.Any();
    public string NoCodesContent => content.GetString(HomePageContent.NoCodesText);
    public string ModalTitleContent => string.Format(content.GetString(HomePageContent.ModalTitleTemplate), SelectedItem?.Name);
    public string ModalBodyContent => string.Format(content.GetString(HomePageContent.ModalBodyTemplate), SelectedItem?.Description);
    public string DeleteButtonContent => content.GetString(HomePageContent.DeleteButtonText);
    public string CancelButtonContent => content.GetString(HomePageContent.CancelButtonText);
    public string SearchPlaceholderText => content.GetString(HomePageContent.SearchPlaceholderText);

    public async Task CopyToClipboard(string code)
    {
        await JSRuntime.InvokeVoidAsync("navigator.clipboard.writeText", code);
    }
    public void OpenDeleteModal(TwoFactorCode code)
    {
        SelectedItem = code;
        IsModalVisible = true;
    }

    public void CloseModal()
    {
        IsModalVisible = false;
        SelectedItem = null;
    }

    public async Task DeleteSelectedCode()
    {
        if (SelectedItem is not null)
        {
            IsDeleting = true;
            await Repository.Delete(SelectedItem.Id);
            await GetCodes();
        }
        CloseModal();
        IsDeleting = false;
    }

    public async Task GetCodes()
    {
        TwoFactorCodesBK = new(await Repository.GetTwoFactorCodes());
        RefreshCodes();

    }

    public void RefreshCodes()
    {
        foreach (var item in TwoFactorCodesBK)
        {
            item.CurrentCode = TOTPGeneratorHelper.GenerateTOTP(item.SharedKey);
        }
        ExecuteSearch();
    }
}
