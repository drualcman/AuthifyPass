namespace AuthifyPass.Client.Core.Interfaces;
public interface IHomeViewModel
{
    string SearchText { get; set; }
    bool IsModalVisible { get; set; }
    bool IsDeleting { get; }
    bool HasCodes { get; }
    string NoCodesContent { get; }
    string ModalTitleContent { get; }
    string ModalBodyContent { get; }
    string DeleteButtonContent { get; }
    string CancelButtonContent { get; }
    string SearchPlaceholderText { get; }
    IEnumerable<TwoFactorCode> TwoFactorCodes { get; }
    TwoFactorCode? SelectedItem { get; }
    Task GetCodes();
    void RefreshCodes();
    Task CopyToClipboard(string code);
    void OpenDeleteModal(TwoFactorCode code);
    void CloseModal();
    Task DeleteSelectedCode();
}
