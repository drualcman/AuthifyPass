namespace AuthifyPass.API.Helpers;

internal static class HeaderHelper
{
    public static string? GetSharedKeyHeader(HttpContext context) => GetHeader(context, "x-authify-key");

    public static string? GetHeader(HttpContext context, string headerName)
    {
        string result = string.Empty;
        if (context.Request.Headers.TryGetValue(headerName, out var headerValue))
        {
            result = headerValue.ToString();
        }
        return result;
    }
}
