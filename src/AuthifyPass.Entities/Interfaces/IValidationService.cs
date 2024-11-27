namespace AuthifyPass.Entities.Interfaces;
public interface IValidationService<T>
{
    IValidationRules<T, TProperty> AddRuleFor<TProperty>(
        Expression<Func<T, TProperty>> expresion);

    ICollectionValidationRules<T, TProperty> AddRuleForEch<TProperty>(
        Expression<Func<T, IEnumerable<TProperty>>> expresion);

    Task<IEnumerable<ValidationError>> Validate(T model);
}
