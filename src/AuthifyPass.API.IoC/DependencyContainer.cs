namespace Microsoft.Extensions.DependencyInjection;
public static class DependencyContainer
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddEntityServices();
        services.AddDbContextServices();
        services.AddRepositoriesServices();
        services.AddValidationService();
        services.AddUseCases();
        return services;
    }


}
