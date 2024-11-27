namespace AuthifyPass.API.Core.ExceptionHandlers;
internal class UnhandledExceptionHandler(ILogger<UnhandledExceptionHandler> Logger,
    IStringLocalizer<ExceptionMessages> localizer) : Microsoft.AspNetCore.Diagnostics.IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        ProblemDetails details = new ProblemDetails();

        details.Status = StatusCodes.Status503ServiceUnavailable;
        details.Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.4";
        details.Title = localizer[nameof(ExceptionMessages.UnhandledExceptionTitle)];
        details.Detail = localizer[nameof(ExceptionMessages.UnhandledExceptionDetail)];
        details.Instance = $"{nameof(ProblemDetails)}/{exception.GetType().Name}";

        Logger.LogError(exception, localizer[nameof(ExceptionMessages.UnhandledExceptionTitle)]);

        await httpContext.WriteProblemDetailsAsync(details);
        return true;
    }
}
