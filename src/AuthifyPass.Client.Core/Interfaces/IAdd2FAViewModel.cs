namespace AuthifyPass.Client.Core.Interfaces;
public interface IAdd2FAViewModel<DtoType> : IViewModelToDto<DtoType>
{
    string Description { get; set; }
    string Name { get; set; }
    string ClientId { get; set; }
    string SharedKey { get; set; }
    Task<bool> AddCode();
    IModelValidatorHub<IAdd2FAViewModel<DtoType>> Validator { get; }
}
