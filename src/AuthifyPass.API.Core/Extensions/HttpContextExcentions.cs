namespace AuthifyPass.API.Core.Extensions;
internal static class HttpContextExcentions
{
    public static async ValueTask WriteProblemDetailsAsync(this HttpContext context, ProblemDetails details)
    {
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = details.Status.Value;
        Stream stream = context.Response.Body;
        await JsonSerializer.SerializeAsync(stream, details);
    }
}
