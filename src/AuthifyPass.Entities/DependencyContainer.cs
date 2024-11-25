using AuthifyPass.Entities.DTOs;
using AuthifyPass.Entities.Interfaces;
using AuthifyPass.Entities.Services;
using AuthifyPass.Entities.Validators;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection;
public static class DependencyContainer
{
    public static IServiceCollection AddEntityServices(this IServiceCollection services)
    {
        services.TryAddScoped(typeof(IModelValidatorHub<>), typeof(ModelValidatorHub<>));
        services.TryAddScoped<IModelValidator<RegisterClientDto>, RegisterClientValidator>();
        services.AddLocalization();
        return services;
    }

    public static IServiceCollection AddModelValidator<ModelType, ModelValidatorType>(this IServiceCollection services)
        where ModelValidatorType : class, IModelValidator<ModelType>
    {
        services.AddEntityServices();
        services.TryAddScoped<IModelValidator<ModelType>, ModelValidatorType>();
        return services;
    }


}
