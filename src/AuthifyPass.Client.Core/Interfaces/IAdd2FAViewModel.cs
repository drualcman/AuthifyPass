namespace AuthifyPass.Client.Core.Interfaces;
public interface IAdd2FAViewModel<DtoType> : IViewModelToDto<DtoType>
{
    string Title { get; set; }
    string Name { get; set; }
    string ClientId { get; set; }
    string SharedKey { get; set; }

    IModelValidatorHub<IAdd2FAViewModel<DtoType>> Validator { get; }
}
