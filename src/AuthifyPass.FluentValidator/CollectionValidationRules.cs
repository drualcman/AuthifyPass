using AuthifyPass.Entities.Abstractions;

namespace AuthifyPass.FluentValidator;
internal class CollectionValidationRules<T, TProperties>(IRuleBuilderInitialCollection<T, TProperties> RuleBuilderInitialCollection)
    : ICollectionValidationRules<T, TProperties>
{
    public ICollectionValidationRules<T, TProperties> SetValidator(IModelValidator<TProperties> validator)
    {
        AbstractModelValidator<TProperties> modelValidator = validator as AbstractModelValidator<TProperties>;
        FluentValidationService<TProperties> validationService = modelValidator.ValidatorService as FluentValidationService<TProperties>;
        RuleBuilderInitialCollection.SetValidator(validationService.Wrapper);
        return this;
    }
}
