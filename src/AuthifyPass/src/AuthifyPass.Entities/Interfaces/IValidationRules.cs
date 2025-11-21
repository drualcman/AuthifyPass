namespace AuthifyPass.Entities.Interfaces;
public interface IValidationRules<T, TProperty>
{
    IValidationRules<T, TProperty> NotEmpty(string errorMessage);
    IValidationRules<T, TProperty> NotNull(string errorMessage);
    IValidationRules<T, TProperty> Must(Func<TProperty, bool> predicate, string errorMessage);
    IValidationRules<T, TProperty> StopOnFirstFailure();
    IValidationRules<T, TProperty> GreaterThan<TValue>(TValue valueToCompare, string errorMessage)
        where TValue : TProperty, IComparable<TValue>, IComparable;
    IValidationRules<T, TProperty> Equal(Expression<Func<T, TProperty>> expresion, string errorMessage);
    IValidationRules<T, string> Length(int lenght, string errorMessage);
    IValidationRules<T, string> MaximumLength(int lenght, string errorMessage);
    IValidationRules<T, string> MinimumLength(int lenght, string errorMessage);
    IValidationRules<T, string> EmailAddress(string errorMessage);
}
