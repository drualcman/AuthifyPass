namespace AuthifyPass.Views.Services;
internal class Repository(DatabaseContext context) : IRepository
{
    public async Task AddTwoFactorCode(TwoFactorCode twoFactorCode)
    {
        TwoFactorCodeModel model = new TwoFactorCodeModel
        {
            Id = twoFactorCode.Id,
            Title = twoFactorCode.Title,
            Name = twoFactorCode.Name,
            ClientId = twoFactorCode.ClientId,
            SharedKey = twoFactorCode.SharedKey,
            CreatedAt = DateTime.UtcNow
        };
        await context.Codes.AddAsync(model);
    }

    public async Task Delete(int id) => await context.Codes.DeleteAsync(id);

    public async Task<IEnumerable<TwoFactorCode>> GetTwoFactorCodes()
    {
        var codes = await context.Codes.SelectAsync();
        return codes.Select(code => new TwoFactorCode
        {
            Id = code.Id,
            Title = code.Title,
            Name = code.Name,
            ClientId = code.ClientId,
            SharedKey = code.SharedKey,
            CreatedAt = DateTime.UtcNow,
            CurrentCode = "algo"
        });
    }
}
