﻿namespace Microsoft.Extensions.DependencyInjection;
public static class DependencyContainer
{
    public static IServiceCollection AddBackendServices(this IServiceCollection services,
        Action<DataBaseOptions> databaseOptions,
        Action<HttpClient> configureHttpClient = null)
    {
        services.AddValidationExceptionsHandlers();
        services.AddDbContextServices(databaseOptions);
        services.AddRepositoriesServices();
        services.AddUseCasesServices();
        services.AddViewsServices(configureHttpClient);
        return services;
    }
}
