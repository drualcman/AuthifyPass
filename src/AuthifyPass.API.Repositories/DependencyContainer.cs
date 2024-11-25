using AuthifyPass.API.Core.Interfaces;
using AuthifyPass.API.Repositories;

namespace Microsoft.Extensions.DependencyInjection;
public static class DependencyContainer
{
    public static IServiceCollection AddRepositoriesServices(this IServiceCollection services)
    {
        services.AddScoped<IClientRepository, ClientRepository>();
        return services;
    }


}
