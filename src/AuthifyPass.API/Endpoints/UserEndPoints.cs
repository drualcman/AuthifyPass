using AuthifyPass.API.Core.Interfaces.UseCases.ValidateUserCode;

namespace AuthifyPass.API.Endpoints;

public static class UserEndPoints
{
    public static WebApplication UseUserEndPoints(this WebApplication app)
    {
        RouteGroupBuilder mainGroup = app.MapGroup("user").WithTags("User");

        mainGroup.MapPost("", async (RegisterUserClientDto data, HttpContext context,
                        IRegisterUserInputPort input) => Results.Ok(await input.RegisterUserAsync(data, HeaderHelper.GetSharedKeyHeader(context))))
            .Produces<RegisterUserClientResponseDto>(StatusCodes.Status200OK);
        mainGroup.MapPost("/validate-code", async (ValidateUserCodeDto data, HttpContext context,
                        IValidateUserCodeInputPort input) => Results.Ok(await input.ValidateUserCode(data, HeaderHelper.GetSharedKeyHeader(context))))
            .Produces<bool>(StatusCodes.Status200OK);
        return app;
    }
}
