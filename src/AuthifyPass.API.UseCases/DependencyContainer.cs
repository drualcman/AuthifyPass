using AuthifyPass.API.Core.Interfaces.UseCases.RegisterClient;
using AuthifyPass.API.UseCases.RegisterClient;

namespace Microsoft.Extensions.DependencyInjection;
public static class DependencyContainer
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<IRegisterClientController, RegisterClientController>();
        services.AddScoped<IRegisterClientInputPort, RegisterClientInteractor>();
        services.AddScoped<IRegisterClientOutputPort, RegisterClientPresenter>();
        return services;
    }


}
