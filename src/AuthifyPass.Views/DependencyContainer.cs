namespace Microsoft.Extensions.DependencyInjection;
public static partial class DependencyContainer
{
    public static IServiceCollection AddViewsServices(this IServiceCollection services, Action<HttpClient> configureHttpClient = null)
    {
        services.TryAddTransient<ExceptionDelegatingHandler>();
        services.AddBlazorIndexedDbContext<DatabaseContext>();
        services.AddHttpClient<IRepository, Repository>(client =>
        {
            configureHttpClient?.Invoke(client);
        });
        services.AddSingleton<ICameraService<TwoFactorCode>, CameraService<TwoFactorCode>>();
        return services;
    }
}
