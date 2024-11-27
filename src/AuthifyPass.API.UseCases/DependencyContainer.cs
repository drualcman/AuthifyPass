namespace Microsoft.Extensions.DependencyInjection;
public static class DependencyContainer
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.AddModelValidator<ValidateUserCodeDto, ValidateUserCodeDtoValidator>();
        services.AddModelValidator<RegisterUserClientDto, RegisterUserClientDtoValidator>();
        services.AddScoped<IRegisterClientController, RegisterClientController>();
        services.AddScoped<IRegisterClientInputPort, RegisterClientInteractor>();
        services.AddScoped<IRegisterClientOutputPort, RegisterClientPresenter>();
        services.AddScoped<IRegisterUserInputPort, RegisterUserInteractor>();
        services.AddScoped<IValidateUserCodeInputPort, ValidateUserCodeInteractor>();
        return services;
    }


}
