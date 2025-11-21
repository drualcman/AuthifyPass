namespace AuthifyPass.FluentValidator;
internal class FluentValidationService<T> : IValidationService<T>
{
    internal readonly AbstracValidatorImplementation<T> Wrapper =
        new AbstracValidatorImplementation<T>();

    public IValidationRules<T, TProperty> AddRuleFor<TProperty>(Expression<Func<T, TProperty>> expresion)
        => new ValidationRules<T, TProperty>(Wrapper.RuleFor(expresion));
    public ICollectionValidationRules<T, TProperty> AddRuleForEch<TProperty>(Expression<Func<T, IEnumerable<TProperty>>> expresion)
        => new CollectionValidationRules<T, TProperty>(Wrapper.RuleForEach(expresion));

    public async Task<IEnumerable<ValidationError>> Validate(T model)
    {
        ValidationResult result = await Wrapper.ValidateAsync(model);
        IEnumerable<ValidationError> errors = default;
        if (!result.IsValid)
        {
            errors = result.Errors.Select(e => new ValidationError(e.PropertyName, e.ErrorMessage));
        }
        return errors;
    }
}
