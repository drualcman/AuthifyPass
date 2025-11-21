namespace AuthifyPass.Views.Validators;
public class ModelValidator<T> : ComponentBase
{
    [CascadingParameter] EditContext EditContext { get; set; }

    [Parameter] public IModelValidatorHub<T> Validator { get; set; }

    ValidationMessageStore ValidationMessageStore;
    FieldIdentifier GetFieldIdentifier(object model, string propertyName)
    {
        char[] PropertyNameSeparators = ['.', '['];
        object NewModel = model;

        string propertyPath = propertyName;

        int separatorIndex;
        string token = null;

        do
        {
            separatorIndex = propertyPath.IndexOfAny(PropertyNameSeparators);
            if (separatorIndex >= 0)
            {
                token = propertyPath.Substring(0, separatorIndex);
                propertyPath = propertyPath.Substring(separatorIndex + 1);
                if (token.EndsWith("]"))
                {
                    token = token.Substring(0, token.Length - 1);
                    PropertyInfo propertyInfo = NewModel.GetType().GetProperty("Item");
                    Type indexedType = propertyInfo.GetIndexParameters()[0].ParameterType;
                    var indexValue = Convert.ChangeType(token, indexedType);        //convertir a un tipo cualquiera, en este caso siempre int
                    NewModel = propertyInfo.GetValue(NewModel, new object[] { indexValue });
                }
                else
                {
                    PropertyInfo propertyInfo = NewModel.GetType().GetProperty(token);
                    NewModel = propertyInfo.GetValue(NewModel);
                }
                token = null;
            }
        } while (separatorIndex >= 0);
        return new FieldIdentifier(NewModel, token ?? propertyPath);
    }

    public void AddErrors(IEnumerable<ValidationError> errors)
    {
        ValidationMessageStore.Clear();
        foreach (ValidationError error in errors)
        {
            FieldIdentifier fieldIdentifier = GetFieldIdentifier(EditContext.Model, error.PropertyName);
            ValidationMessageStore.Add(fieldIdentifier, error.Message);
        }
        EditContext.NotifyValidationStateChanged();
    }

    async void ValidationRequested(object sender, ValidationRequestedEventArgs args)
    {
        bool isValid = await Validator.Validate((T)EditContext.Model);
        if (isValid)
        {
            ValidationMessageStore.Clear();
            EditContext.NotifyValidationStateChanged();
        }
        else
        {
            AddErrors(Validator.Errors);
        }
    }

    async void FieldChange(object sender, FieldChangedEventArgs args)
    {
        ValidationMessageStore.Clear(args.FieldIdentifier);
        bool isValid = await Validator.Validate((T)EditContext.Model);
        if (!isValid)
        {
            foreach (ValidationError error in Validator.Errors)
            {
                FieldIdentifier fieldIdentifier = GetFieldIdentifier(EditContext.Model, error.PropertyName);
                if (fieldIdentifier.FieldName == args.FieldIdentifier.FieldName &&
                   fieldIdentifier.Model == args.FieldIdentifier.Model)
                {
                    ValidationMessageStore.Add(fieldIdentifier, error.Message);
                }
            }
        }
        //EditContext.NotifyFieldChanged(args.FieldIdentifier);
        EditContext.NotifyValidationStateChanged();
    }

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        EditContext previousEditContext = EditContext;
        await base.SetParametersAsync(parameters);
        if (EditContext != previousEditContext)
        {
            ValidationMessageStore = new ValidationMessageStore(EditContext);
            EditContext.OnValidationRequested += ValidationRequested;
            EditContext.OnFieldChanged += FieldChange;
        }
    }
}
