namespace Microsoft.Extensions.DependencyInjection;
public static partial class DependencyContainer
{
    public static IServiceCollection AddViewsServices(this IServiceCollection services, Action<HttpClient> configureHttpClient = null)
    {
        services.AddValidationService();
        services.AddEntityServices();
        services.AddSingleton<IToastMessage, ToastMessageService>();
        services.AddSingleton<ICameraService<TwoFactorCode>, CameraService<TwoFactorCode>>();
        services.AddBlazorIndexedDbContext<DatabaseContext>();
        services.AddHttpClient<IRepository, Repository>(client =>
        {
            configureHttpClient?.Invoke(client);
        });
        services.AddTransient<IAdd2FAViewModel<TwoFactorCode>, Add2FAViewModel>();
        services.AddScoped<IHomeViewModel, HomeViewModel>();
        services.AddModelValidator<TwoFactorCode, TwoFactorCodeValidator>();
        services.AddModelValidator<IAdd2FAViewModel<TwoFactorCode>, TwoFactorCodeModelValidator>();
        services.TryAddTransient<ExceptionDelegatingHandler>();
        return services;
    }
}
