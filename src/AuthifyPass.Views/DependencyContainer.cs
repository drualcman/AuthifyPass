using AuthifyPass.Views.Validators;
using AuthifyPass.Views.ViewModel;

namespace Microsoft.Extensions.DependencyInjection;
public static partial class DependencyContainer
{
    public static IServiceCollection AddViewsServices(this IServiceCollection services, Action<HttpClient> configureHttpClient = null)
    {
        services.AddEntityServices();
        services.AddValidationService();
        services.AddModelValidator<TwoFactorCode, TwoFactorCodeValidator>();
        services.AddScoped<Add2FAViewModel>();
        services.AddModelValidator<Add2FAViewModel, TwoFactorCodeModelValidator>();
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
