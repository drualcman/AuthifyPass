namespace Microsoft.Extensions.DependencyInjection;
public static partial class DependencyContainer
{
    public static IServiceCollection AddValidationService(this IServiceCollection services)
    {
        services.AddScoped(typeof(IValidationService<>), typeof(FluentValidationService<>));
        return services;
    }
}
