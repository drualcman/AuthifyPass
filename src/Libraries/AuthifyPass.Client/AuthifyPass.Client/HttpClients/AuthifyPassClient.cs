namespace AuthifyPass.Client.HttpClients;

internal class AuthifyPassClient : ClientBase, IAuthifyPassClient
{
    private readonly AuthifyPassOptions Options;
    private readonly HttpClient Client;

    public AuthifyPassClient(IOptions<AuthifyPassOptions> options, IHttpClientFactory clientFactory, ILogger logger = null) : base(logger)
    {
        Options = options.Value;
        Client = clientFactory.CreateClient();
        Client.BaseAddress = new Uri(Options.BaseUrl);
        Client.DefaultRequestHeaders.Add(Options.Header, Options.Secret);
    }

    public async Task<AuthifyPassResponse> RequestUser2FactorQRAsync(string userId, string userName, CancellationToken cancellationToken = default)
    {
        var request = new
        {
            clientId = Options.ClientID,
            userId = userId,
            userName = userName
        };
        using HttpResponseMessage response = await Client.PostAsJsonAsync(Options.UserEndpoint, request);
        await ThrowIfNotSuccessCore(response);
        return await response.Content.ReadFromJsonAsync<AuthifyPassResponse>(cancellationToken: cancellationToken);
    }

    public async Task RequestDeleteUser2FactorAsync(string userId, CancellationToken cancellationToken = default)
    {
        var request = new
        {
            clientId = Options.ClientID,
            userId = userId
        };
        using HttpResponseMessage response = await Client.DeleteAsync($"{Options.UserEndpoint}/{userId}");
        await ThrowIfNotSuccessCore(response);
    }

    public async Task<bool> ValidateUser2FactorAsync(string userId, string code, CancellationToken cancellationToken = default)
    {
        var request = new
        {
            clientId = Options.ClientID,
            userId = userId,
            userCode = code
        };
        using HttpResponseMessage response = await Client.PostAsJsonAsync($"{Options.UserEndpoint}/validate-code", request);
        await ThrowIfNotSuccessCore(response);
        return await response.Content.ReadFromJsonAsync<bool>(cancellationToken: cancellationToken);
    }
}