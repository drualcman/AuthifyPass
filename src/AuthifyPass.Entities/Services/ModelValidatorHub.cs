using AuthifyPass.Entities.Enums;
using AuthifyPass.Entities.Interfaces;
using AuthifyPass.Entities.ValueObjects;

namespace AuthifyPass.Entities.Services;
internal class ModelValidatorHub<ModelType>(
    IEnumerable<IModelValidator<ModelType>> Validators
    ) : IModelValidatorHub<ModelType>
{
    public IEnumerable<ValidationError> Errors { get; private set; }

    public async Task<bool> Validate(ModelType model)
    {
        List<ValidationError> currentErrors = [];

        List<IModelValidator<ModelType>> validators = Validators
            .Where(v => v.Constraint == ValidationConstraint.AlwaysValidate)
            .ToList();

        validators.AddRange(Validators
            .Where(v => v.Constraint == ValidationConstraint.ValidateIfThereAreNoPreviousErrors));

        foreach (var validator in validators)
        {
            if (validator.Constraint == ValidationConstraint.AlwaysValidate ||
                    !currentErrors.Any())
            {
                if (!await validator.Validate(model))
                {
                    currentErrors.AddRange(validator.Errors);
                }
            }
        }
        Errors = currentErrors;
        return !Errors.Any();
    }
}
