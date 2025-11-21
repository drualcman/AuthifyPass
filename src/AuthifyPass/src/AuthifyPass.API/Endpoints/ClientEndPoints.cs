namespace AuthifyPass.API.Endpoints;

public static class ClientEndPoints
{
    public static WebApplication MapClientEndPoints(this WebApplication app)
    {
        RouteGroupBuilder mainGroup = app.MapGroup("client").WithTags("Client");

        mainGroup.MapPost("", async (RegisterClientDto data,
                    IRegisterClientController controller) =>
                Results.Ok(await controller.CreateClientAsync(data)))
            .Produces<RegisterClientResponseDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);
        mainGroup.MapDelete("{id}", async (string id, HttpContext context,
                    IDeleteClientController controller) =>
                await controller.DeleteClient(id, HeaderHelper.GetSharedKeyHeader(context)))
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

        return app;
    }
}
