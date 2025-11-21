namespace AuthifyPass.Client.Core.Interfaces;
public interface IRepository
{
    Task<IEnumerable<TwoFactorCode>> GetTwoFactorCodes();
    Task AddTwoFactorCode(TwoFactorCode twoFactorCode);
    Task Delete(int id);
}
