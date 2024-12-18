﻿namespace Microsoft.Extensions.DependencyInjection;
public static partial class DependencyContainer
{
    public static IServiceCollection AddRepositoriesServices(this IServiceCollection services)
    {
        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<IUserSecretRepository, UserSecretRepository>();
        services.AddScoped<IQRGeneratorRepository, QRGeneratorRepository>();
        return services;
    }


}
