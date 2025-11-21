using DevsTv.Entities.Models;

namespace AuthifyPass.Client.HttpClients
{
    internal abstract class ClientBase
    {
        private readonly ILogger Logger;
        protected static readonly CultureInfo Invariant = CultureInfo.InvariantCulture;

        public ClientBase(ILogger logger = null)
        {
            Logger = logger;
        }

        // Returns ProblemDetails only when there is an HTTP error
        protected async Task<ProblemDetails> ThrowIfNotSuccessCore(HttpResponseMessage response)
        {
            ProblemDetails result = null;

            if (!response.IsSuccessStatusCode)
            {
                // Unauthorized response is a special case
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    string reason = response.ReasonPhrase ?? "Unauthorized";

                    Logger?.LogWarning(
                        "HTTP 401 Unauthorized. Reason: {Reason}",
                        reason
                    );

                    result = new ProblemDetails();
                    result.Status = 401;
                    result.Title = reason;
                }
                else
                {
                    string rawContent = await response.Content.ReadAsStringAsync();
                    bool isEmpty = string.IsNullOrWhiteSpace(rawContent);

                    if (isEmpty)
                    {
                        string reason = response.ReasonPhrase ?? "Request failed";

                        Logger?.LogError(
                            "HTTP request failed with empty body. Status: {Status}, Reason: {Reason}",
                            response.StatusCode,
                            reason
                        );

                        result = new ProblemDetails();
                        result.Status = (int)response.StatusCode;
                        result.Title = reason;
                    }
                    else
                    {
                        try
                        {
                            Dictionary<string, object> deserialized =
                                JsonSerializer.Deserialize<Dictionary<string, object>>(rawContent);

                            if (deserialized != null)
                            {
                                ProblemDetails problem = CreateProblemDetails(deserialized);
                                string builtMessage = BuildMessage(problem, deserialized);

                                Logger?.LogError(
                                    "HTTP request failed with ProblemDetails. Status: {Status}, Title: {Title}, Detail: {Detail}",
                                    problem.Status,
                                    problem.Title,
                                    problem.Detail
                                );

                                // Always provide a fully constructed ProblemDetails
                                result = new ProblemDetails();
                                result.Status = problem.Status;
                                result.Title = builtMessage;
                                result.Detail = problem.Detail;
                                result.Instance = problem.Instance;
                                result.Type = problem.Type;
                            }
                        }
                        catch (JsonException jsonEx)
                        {
                            string reason = response.ReasonPhrase ?? "Request failed";

                            Logger?.LogError(
                                "Malformed JSON in HTTP response. Error: {Error}, Reason: {Reason}, Content: {Content}",
                                jsonEx.Message,
                                reason,
                                rawContent
                            );

                            result = new ProblemDetails();
                            result.Status = (int)response.StatusCode;
                            result.Title = reason;
                            result.Detail = rawContent;
                        }
                    }
                }
            }

            return result;
        }


        private ProblemDetails CreateProblemDetails(Dictionary<string, object> source)
        {
            ProblemDetails details = new ProblemDetails();

            details.Type = source.ContainsKey("type") ? source["type"]?.ToString() : null;
            details.Title = source.ContainsKey("title") ? source["title"]?.ToString() : "Request error";

            int parsedStatus = 0;
            bool ok = source.ContainsKey("status") &&
                      int.TryParse(source["status"]?.ToString(), NumberStyles.Any, Invariant, out parsedStatus);

            details.Status = ok ? parsedStatus : 400;

            details.Detail = source.ContainsKey("detail") ? source["detail"]?.ToString() : null;
            details.Instance = source.ContainsKey("instance") ? source["instance"]?.ToString() : null;

            return details;
        }

        private string BuildMessage(ProblemDetails problem, Dictionary<string, object> data)
        {
            string message = problem.Title;

            if (!string.IsNullOrWhiteSpace(problem.Detail))
            {
                message = message + " | " + problem.Detail;
            }

            bool isValidation =
                !string.IsNullOrWhiteSpace(problem.Instance) &&
                problem.Instance.Contains("ValidationException", StringComparison.OrdinalIgnoreCase);

            if (isValidation)
            {
                foreach (KeyValuePair<string, object> kv in data)
                {
                    bool isStandard =
                        kv.Key == "type" ||
                        kv.Key == "title" ||
                        kv.Key == "status" ||
                        kv.Key == "detail" ||
                        kv.Key == "instance";

                    if (!isStandard)
                    {
                        message = message + " | " + kv.Key + ": " + kv.Value;
                    }
                }
            }

            return message;
        }
    }
}