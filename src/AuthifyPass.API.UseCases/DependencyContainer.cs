namespace Microsoft.Extensions.DependencyInjection;
public static partial class DependencyContainer
{
    public static IServiceCollection AddUseCasesServices(this IServiceCollection services)
    {
        services.AddScoped<IRegisterClientController, RegisterClientController>();
        services.AddScoped<IRegisterClientInputPort, RegisterClientInteractor>();
        services.AddScoped<IRegisterClientOutputPort, RegisterClientPresenter>();
        services.AddScoped<IRegisterUserInputPort, RegisterUserInteractor>();
        services.AddScoped<IValidateUserCodeInputPort, ValidateUserCodeInteractor>();
        services.AddScoped<IDeleteClientController, DeleteClientController>();
        services.AddScoped<IDeleteClientInputPort, DeleteClientInteractor>();
        services.AddScoped<IDeleteUserController, DeleteUserController>();
        services.AddScoped<IDeleteUserInputPort, DeleteUserInteractor>();
        return services;
    }


}
