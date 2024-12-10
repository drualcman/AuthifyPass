namespace AuthifyPass.FluentValidator;
internal class ValidationRules<T, TProperty>(IRuleBuilderInitial<T, TProperty> RuleBuilderInitial)
    : IValidationRules<T, TProperty>
{
    IRuleBuilder<T, string> StringRuleBuilder => (IRuleBuilder<T, string>)RuleBuilderInitial;

    IValidationRules<T, string> ThisAsStringValidationRules => (IValidationRules<T, string>)this;

    public IValidationRules<T, string> EmailAddress(string errorMessage)
    {
        StringRuleBuilder
            .EmailAddress()
            .WithMessage(errorMessage);
        return ThisAsStringValidationRules;
    }

    public IValidationRules<T, TProperty> Equal(Expression<Func<T, TProperty>> expresion, string errorMessage)
    {
        RuleBuilderInitial
            .Equal(expresion)
            .WithMessage(errorMessage);
        return this;
    }

    public IValidationRules<T, TProperty> GreaterThan<TValue>(TValue valueToCompare, string errorMessage) where TValue : TProperty, IComparable<TValue>, IComparable
    {
        IRuleBuilder<T, TValue> ruleBuilder = (IRuleBuilder<T, TValue>)RuleBuilderInitial;
        ruleBuilder
            .GreaterThan(valueToCompare)
            .WithMessage(errorMessage);
        return this;
    }

    public IValidationRules<T, string> Length(int lenght, string errorMessage)
    {
        StringRuleBuilder
            .Length(lenght)
            .WithMessage(errorMessage);
        return ThisAsStringValidationRules;
    }

    public IValidationRules<T, string> MaximumLength(int lenght, string errorMessage)
    {
        StringRuleBuilder
            .MaximumLength(lenght)
            .WithMessage(errorMessage);
        return ThisAsStringValidationRules;
    }

    public IValidationRules<T, string> MinimumLength(int lenght, string errorMessage)
    {
        StringRuleBuilder
            .MinimumLength(lenght)
            .WithMessage(errorMessage);
        return ThisAsStringValidationRules;
    }

    public IValidationRules<T, TProperty> Must(Func<TProperty, bool> predicate, string errorMessage)
    {
        RuleBuilderInitial.Must(predicate).WithMessage(errorMessage);
        return this;
    }

    public IValidationRules<T, TProperty> NotEmpty(string errorMessage)
    {
        RuleBuilderInitial.Must(v => !string.IsNullOrWhiteSpace(v?.ToString() ?? "")).WithMessage(errorMessage);
        return this;
    }

    public IValidationRules<T, TProperty> NotNull(string errorMessage)
    {
        RuleBuilderInitial.NotNull().WithMessage(errorMessage);
        return this;
    }

    public IValidationRules<T, TProperty> StopOnFirstFailure()
    {
        RuleBuilderInitial.Cascade(CascadeMode.Stop);
        return this;
    }
}
