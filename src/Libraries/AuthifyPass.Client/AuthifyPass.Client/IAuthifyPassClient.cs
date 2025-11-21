namespace AuthifyPass.Client;

public interface IAuthifyPassClient
{
    Task RequestDeleteUser2FactorAsync(string userId, CancellationToken cancellationToken = default);
    Task<AuthifyPassResponse> RequestUser2FactorQRAsync(string userId, string userName, CancellationToken cancellationToken = default);
    Task<bool> ValidateUser2FactorAsync(string userId, string code, CancellationToken cancellationToken = default);
}