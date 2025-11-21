namespace Microsoft.Extensions.DependencyInjection;
public static partial class DependencyContainer
{
    public static IServiceCollection AddEntityServices(this IServiceCollection services)
    {
        services.AddLocalization();
        services.AddSingleton<IIdentifierGenerator, IdentifierGenerator>();
        services.AddModelValidator<RegisterClientDto, RegisterClientValidator>();
        return services;
    }

    public static IServiceCollection AddModelValidator(this IServiceCollection services)
    {
        services.TryAddScoped(typeof(IModelValidatorHub<>), typeof(ModelValidatorHub<>));
        return services;
    }

    public static IServiceCollection AddModelValidator<ModelType, ModelValidatorType>(this IServiceCollection services)
        where ModelValidatorType : class, IModelValidator<ModelType>
    {
        services.AddModelValidator();
        services.TryAddScoped<IModelValidator<ModelType>, ModelValidatorType>();
        return services;
    }


}
