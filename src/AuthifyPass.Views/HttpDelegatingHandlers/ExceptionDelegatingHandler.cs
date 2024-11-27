namespace AuthifyPass.Views.HttpDelegatingHandlers;

public class ExceptionDelegatingHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            string errorMessage = await response.Content.ReadAsStringAsync();

            string source = string.Empty;
            string message = string.Empty;

            IEnumerable<ValidationError> errors = null;
            bool IsValidProblemDetails = false;

            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.Unauthorized:
                case System.Net.HttpStatusCode.Forbidden:
                    errorMessage = $"{(int)response.StatusCode} {response.ReasonPhrase}";
                    break;
                default:
                    try
                    {
                        string contentType = response.Content.Headers.ContentType.MediaType;
                        JsonElement jsonResponse = JsonSerializer.Deserialize<JsonElement>(errorMessage);
                        if (contentType.ToLower() == "application/problem+json" &&
                            TryGetProerty(jsonResponse, "instance", out JsonElement instanceValue))
                        {
                            string value = instanceValue.ToString();
                            if (value.ToLower().StartsWith("problemdetails/"))
                            {
                                source = value;
                                if (TryGetProerty(jsonResponse, "title", out JsonElement titleValue))
                                {
                                    message = titleValue.ToString();
                                }
                                if (TryGetProerty(jsonResponse, "detail", out JsonElement detailValue))
                                {
                                    message = $"{message} {detailValue}".Trim();
                                }
                                if (TryGetProerty(jsonResponse, "errors", out JsonElement errorsValue))
                                {
                                    errors = errorsValue.Deserialize<IEnumerable<ValidationError>>(
                                        new JsonSerializerOptions
                                        {
                                            PropertyNameCaseInsensitive = true
                                        });
                                }

                                IsValidProblemDetails = true;

                            }
                        }
                    }
                    catch { }
                    break;
            }
            if (!IsValidProblemDetails)
            {
                message = errorMessage;
                source = string.Empty;
                errors = null;
            }

            HttpRequestException ex = new HttpRequestException(message, null, response.StatusCode);
            ex.Source = source;
            if (errors is not null)
                ex.Data.Add("Errors", errors);
            throw ex;
        }
        return response;
    }

    bool TryGetProerty(JsonElement element, string propertyName, out JsonElement value)
    {
        bool found = false;
        value = default;

        JsonProperty property = element.EnumerateObject()
            .FirstOrDefault(e => string.Compare(e.Name, propertyName, StringComparison.OrdinalIgnoreCase) == 0);

        if (property.Value.ValueKind != JsonValueKind.Undefined)
        {
            value = property.Value;
            found = true;
        }

        return found;
    }
}
