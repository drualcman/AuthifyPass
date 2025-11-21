namespace Microsoft.Extensions.DependencyInjection;

public static partial class DependencyContainer
{
    public static IServiceCollection AddAuthifyPassClient(this IServiceCollection services, Action<AuthifyPassOptions> options = null)
    {
        if (options is not null)
            services.Configure(options);
        else
            services
                .AddOptions<AuthifyPassOptions>()
                .BindConfiguration(AuthifyPassOptions.SectionKey);
        services.AddScoped<IAuthifyPassClient, AuthifyPassClient>();
        return services;
    }
}
