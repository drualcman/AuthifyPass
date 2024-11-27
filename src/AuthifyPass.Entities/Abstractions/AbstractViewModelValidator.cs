namespace AuthifyPass.Entities.Abstractions;
public abstract class AbstractViewModelValidator<DtoType, ViewModelType>
    (IModelValidatorHub<DtoType> dtoModelValidatorHub, ValidationConstraint constraint)
    : IModelValidator<ViewModelType>
{
    public ValidationConstraint Constraint => constraint;

    public IEnumerable<ValidationError> Errors => dtoModelValidatorHub.Errors;

    public virtual DtoType Cast(ViewModelType viewModel)
    {
        DtoType dtoModel = default;

        MethodInfo explicitMethod = typeof(ViewModelType).GetMethod("op_Explicit");
        if (explicitMethod != null)
        {
            dtoModel = (DtoType)explicitMethod.Invoke(
                viewModel, new object[] { viewModel });
        }
        else
        {
            throw new InvalidCastException();
        }

        return dtoModel;
    }

    public async Task<bool> Validate(ViewModelType model) =>
        await dtoModelValidatorHub.Validate(Cast(model));
}
