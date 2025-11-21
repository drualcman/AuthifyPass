namespace Microsoft.Extensions.DependencyInjection;
public static partial class DependencyContainer
{
    public static IServiceCollection AddDbContextServices(this IServiceCollection services,
        Action<DataBaseOptions> options)
    {
        services.Configure(options);
        services.AddScoped<IWritableDbContext, WritableDbContext>();
        services.AddScoped<IReadbleDbContext, ReadableDbContext>();
        return services;
    }


}
