namespace AuthifyPass.Client.Core.Interfaces;
public interface IHomeViewModel
{
    string SearchText { get; set; }
    bool IsModalVisible { get; set; }
    bool IsDeleting { get; }
    bool HasCodes { get; }
    bool IsShareVisible { get; set; }
    bool HasShareQr { get; }
    bool IsConfirmingSecretCopy { get; }
    string ShareOtpAuthUri { get; }
    string ShareQrSvg { get; }
    string NoCodesContent { get; }
    string ModalTitleContent { get; }
    string ModalBodyContent { get; }
    string DeleteButtonContent { get; }
    string CancelButtonContent { get; }
    string SearchPlaceholderText { get; }
    IEnumerable<TwoFactorCode> TwoFactorCodes { get; }
    TwoFactorCode? SelectedItem { get; }
    Task GetCodes();
    Task RefreshCodes();
    Task CopyToClipboard(string code);
    void OpenDeleteModal(TwoFactorCode code);
    void CloseModal();
    Task DeleteSelectedCode();
    string ShareTitleText { get; }
    string ShareHintText { get; }
    string CopyLinkButtonText { get; }
    string CopySecretButtonText { get; }
    string ShareCloseButtonText { get; }
    string ConfirmSecretText { get; }
    string ConfirmYesText { get; }
    string ConfirmNoText { get; }
    void OpenShareModal(TwoFactorCode code);
    void CloseShareModal();
    Task CopyOtpAuthUri();
    void RequestCopySecret();
    Task ConfirmCopySecret();
    void CancelCopySecret();
}
