namespace AuthifyPass.Entities.Abstractions;
public abstract class AbstractViewModelValidator<DtoType, ViewModelType>
    (IModelValidatorHub<DtoType> dtoModelValidatorHub, ValidationConstraint constraint)
    : IModelValidator<ViewModelType> where ViewModelType : IViewModelToDto<DtoType>
{
    public ValidationConstraint Constraint => constraint;

    public IEnumerable<ValidationError> Errors => dtoModelValidatorHub.Errors;

    public async Task<bool> Validate(ViewModelType model) =>
        await dtoModelValidatorHub.Validate(model.ToDto());
}
