namespace AuthifyPass.API.Endpoints;

public static class ClientEndPoints
{
    public static WebApplication UseClientEndPoints(this WebApplication app)
    {
        RouteGroupBuilder mainGroup = app.MapGroup("client").WithTags("Client");

        mainGroup.MapPost("", async (RegisterClientDto data,
                    IRegisterClientController controller) => Results.Ok(await controller.CreateClientAsync(data)))
            .Produces<RegisterClientResponseDto>(StatusCodes.Status200OK);
        mainGroup.MapDelete("{id}", async (string id, HttpContext context, IDeleteClientController controller) =>
        await controller.DeleteClient(id, HeaderHelper.GetSharedKeyHeader(context)));

        return app;
    }
}
