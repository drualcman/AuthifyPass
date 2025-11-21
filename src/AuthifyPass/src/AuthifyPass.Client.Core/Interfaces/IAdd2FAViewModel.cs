namespace AuthifyPass.Client.Core.Interfaces;
public interface IAdd2FAViewModel<DtoType> : IViewModelToDto<DtoType>
{
    string Description { get; set; }
    string Name { get; set; }
    string UserId { get; set; }
    string SharedKey { get; set; }
    Task<bool> AddCode();
    IModelValidatorHub<IAdd2FAViewModel<DtoType>> Validator { get; }
    string TitleText { get; }
    string DescriptionText { get; }
    string AddButtonText { get; }
    string CancelButtonText { get; }
    string FromImageButtonText { get; }
    string ManualButtonText { get; }
    string CompanyText { get; }
    string CodeText { get; }
    string SecretText { get; }
}
