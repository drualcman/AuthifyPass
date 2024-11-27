namespace Microsoft.Extensions.DependencyInjection;
public static class DependencyContainer
{
    public static IServiceCollection AddServices(this IServiceCollection services,
        Action<DataBaseOptions> databaseOptions)
    {
        services.AddEntityServices();
        services.AddDbContextServices(databaseOptions);
        services.AddRepositoriesServices();
        services.AddValidationService();
        services.AddUseCases();
        return services;
    }


}
