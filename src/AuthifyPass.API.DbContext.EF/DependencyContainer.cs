namespace Microsoft.Extensions.DependencyInjection;
public static class DependencyContainer
{
    public static IServiceCollection AddDbContextServices(this IServiceCollection services)
    {
        services.AddScoped<IWritableDbContext, WritableDbContext>();
        return services;
    }


}
