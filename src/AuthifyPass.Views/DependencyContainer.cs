using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Globalization;

namespace Microsoft.Extensions.DependencyInjection;
public static partial class DependencyContainer
{
    public static IServiceCollection AddViewsServices(this IServiceCollection services, Action<HttpClient> configureHttpClient = null)
    {
        services.AddValidationService();
        services.AddEntityServices();
        services.AddSingleton<IToastMessage, ToastMessageService>();
        services.AddSingleton<ICameraService, CameraService>();
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

    public async static Task SetDefaultCulture(this WebAssemblyHost host)
    {
        const string defaultCulture = "es-ES";

        IJSRuntime js = host.Services.GetRequiredService<IJSRuntime>();
        string result = await js.InvokeAsync<string>("blazorCulture.get");
        CultureInfo culture = CultureInfo.GetCultureInfo(defaultCulture);

        if (result == null)
            await js.InvokeVoidAsync("blazorCulture.set", defaultCulture);
        else
            culture = new CultureInfo(result);

        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
    }
}
