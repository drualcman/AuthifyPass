using AuthifyPass.API.Core.Interfaces.UseCases.ValidateUserCode;

namespace AuthifyPass.API.Endpoints;

public static class UserEndPoints
{
    public static WebApplication UseUserEndPoints(this WebApplication app)
    {
        RouteGroupBuilder mainGroup = app.MapGroup("user").WithTags("User");

        mainGroup.MapPost("", async (RegisterUserClientDto data, HttpContext context,
                        IRegisterUserInputPort input) =>
                Results.Ok(await input.RegisterUserAsync(data, HeaderHelper.GetSharedKeyHeader(context))))
            .Produces<RegisterUserClientResponseDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);
        mainGroup.MapPost("/validate-code", async (ValidateUserCodeDto data, HttpContext context,
                        IValidateUserCodeInputPort input) =>
                Results.Ok(await input.ValidateUserCode(data, HeaderHelper.GetSharedKeyHeader(context))))
            .Produces<bool>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);
        mainGroup.MapDelete("{id}", async (string id, HttpContext context,
                        IDeleteUserController controller) =>
                await controller.DeleteUser(id, HeaderHelper.GetSharedKeyHeader(context)))
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);
        return app;
    }
}
