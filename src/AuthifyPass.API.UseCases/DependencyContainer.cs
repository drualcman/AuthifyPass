using AuthifyPass.API.Core.Interfaces;
using AuthifyPass.API.UseCases;

namespace Microsoft.Extensions.DependencyInjection;
public static class DependencyContainer
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<IRegisterClientInputPort, RegisterClientInteractor>();
        return services;
    }


}
