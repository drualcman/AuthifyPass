namespace Microsoft.Extensions.DependencyInjection;
public static class DependencyContainer
{
    public static IServiceCollection AddBackendServices(this IServiceCollection services,
        Action<DataBaseOptions> databaseOptions)
    {
        services.AddEntityServices();
        services.AddValidationService();
        services.AddValidationExceptionsHandlers();
        services.AddDbContextServices(databaseOptions);
        services.AddRepositoriesServices();
        services.AddUseCasesServices();
        services.AddViewsServices();
        return services;
    }


}
