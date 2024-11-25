namespace AuthifyPass.Entities.Interfaces;
public interface ICollectionValidationRules<T, TProperty>
{
    ICollectionValidationRules<T, TProperty> SetValidator(
        IModelValidator<TProperty> validator);
}
