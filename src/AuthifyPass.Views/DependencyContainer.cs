﻿namespace Microsoft.Extensions.DependencyInjection;
public static partial class DependencyContainer
{
    public static IServiceCollection AddViewsServices(this IServiceCollection services)
    {
        services.TryAddTransient<ExceptionDelegatingHandler>();
        services.AddBlazorIndexedDbContext<DatabaseContext>();
        services.AddScoped<IRepository, Repository>();
        return services;
    }


}