namespace Microsoft.Extensions.DependencyInjection;
public static class DependencyContainer
{
    public static IServiceCollection AddValidationExceptionsHandlers(this IServiceCollection services)
    {
        services.AddModelValidator<ValidateUserCodeDto, ValidateUserCodeDtoValidator>();
        services.AddModelValidator<RegisterUserClientDto, RegisterUserClientDtoValidator>();
        services.AddExceptionHandler<ValidationExceptionHandler>();
        services.AddExceptionHandler<UpdateExceptionHandler>();
        services.AddExceptionHandler<UnauthorizedAccessExceptionHandler>();
        services.AddExceptionHandler<UnhandledExceptionHandler>();
        return services;
    }


}
