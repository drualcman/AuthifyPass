namespace AuthifyPass.Views.ViewModels;
internal class HomeViewModel(
    IRepository Repository,
    IJSRuntime JSRuntime,
    IToastMessage ToastMessage,
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
        await ToastMessage.Information(content[nameof(HomePageContent.CodeCopiedText)]);
    }

    private TwoFactorCode? shareItem;

    public bool IsShareVisible { get; set; }
    public string ShareOtpAuthUri { get; private set; } = string.Empty;
    public string ShareQrSvg { get; private set; } = string.Empty;
    public bool HasShareQr => !string.IsNullOrEmpty(ShareQrSvg);
    public string ShareTitleText => content[nameof(HomePageContent.ShareTitleText)];
    public string ShareHintText => content[nameof(HomePageContent.ShareHintText)];
    public string CopyLinkButtonText => content[nameof(HomePageContent.CopyLinkButtonText)];
    public string CopySecretButtonText => content[nameof(HomePageContent.CopySecretButtonText)];
    public string ShareCloseButtonText => content[nameof(HomePageContent.ShareCloseButtonText)];
    public bool IsConfirmingSecretCopy { get; private set; }
    public string ConfirmSecretText => content[nameof(HomePageContent.ConfirmSecretText)];
    public string ConfirmYesText => content[nameof(HomePageContent.ConfirmYesText)];
    public string ConfirmNoText => content[nameof(HomePageContent.ConfirmNoText)];

    public void OpenShareModal(TwoFactorCode code)
    {
        shareItem = code;
        ShareOtpAuthUri = OtpAuthUri.Build(code);
        ShareQrSvg = QrCodeSvgRenderer.RenderSvg(ShareOtpAuthUri);
        IsConfirmingSecretCopy = false;
        IsShareVisible = true;
    }

    public void CloseShareModal()
    {
        IsShareVisible = false;
        shareItem = null;
        ShareOtpAuthUri = string.Empty;
        ShareQrSvg = string.Empty;
        IsConfirmingSecretCopy = false;
    }

    public async Task CopyOtpAuthUri()
    {
        await JSRuntime.InvokeVoidAsync("navigator.clipboard.writeText", ShareOtpAuthUri);
        await ToastMessage.Information(content[nameof(HomePageContent.ShareLinkCopiedText)]);
    }

    public void RequestCopySecret()
    {
        IsConfirmingSecretCopy = true;
    }

    public void CancelCopySecret()
    {
        IsConfirmingSecretCopy = false;
    }

    public async Task ConfirmCopySecret()
    {
        IsConfirmingSecretCopy = false;

        if (shareItem is not null)
        {
            await JSRuntime.InvokeVoidAsync("navigator.clipboard.writeText", shareItem.SharedKey);
            await ToastMessage.Information(content[nameof(HomePageContent.ShareSecretCopiedText)]);
        }
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
        bool deleteSucceeded = true;

        if (SelectedItem is not null)
        {
            IsDeleting = true;

            try
            {
                await Repository.Delete(SelectedItem.Id);
                await GetCodes();
            }
            catch (Exception)
            {
                deleteSucceeded = false;
            }
        }

        CloseModal();
        IsDeleting = false;

        if (!deleteSucceeded)
        {
            await ToastMessage.Warning(content[nameof(HomePageContent.DeleteFailedText)]);
        }
    }

    public async Task GetCodes()
    {
        TwoFactorCodesBK = new(await Repository.GetTwoFactorCodes());
        await RefreshCodes();

    }

    public async Task RefreshCodes()
    {
        List<Task> tasks = [];
        foreach (var item in TwoFactorCodesBK)
        {
            tasks.Add(Task.Run(() =>
            {
                item.CurrentCode = TOTPGeneratorHelper.GenerateTOTP(item.SharedKey, item.Period, item.Digits);
            }));
        }
        await Task.WhenAll(tasks);
        ExecuteSearch();
    }
}
