namespace AuthifyPass.Views.Services;
internal class Repository(DatabaseContext context, HttpClient client) : IRepository
{
    public async Task AddTwoFactorCode(TwoFactorCode twoFactorCode)
    {
        TwoFactorCodeModel model = new TwoFactorCodeModel
        {
            Id = twoFactorCode.Id,
            Title = twoFactorCode.Description,
            Name = twoFactorCode.Name,
            ClientId = twoFactorCode.ClientId,
            SharedKey = twoFactorCode.SharedKey,
            CreatedAt = DateTime.UtcNow
        };
        await context.Codes.AddAsync(model);
    }

    public async Task Delete(int id)
    {
        var codes = await context.Codes.SelectAsync(c => c.Id.Equals(id));
        var code = codes.FirstOrDefault();
        try
        {
            if (code is not null)
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("x-authify-key", code.SharedKey);
                using HttpResponseMessage response = await client.DeleteAsync($"user/{code.ClientId}");
                response.EnsureSuccessStatusCode();
            }
        }
        catch (Exception ex)
        {
            await Console.Out.WriteLineAsync(ex.Message);
        }
        finally
        {
            await context.Codes.DeleteAsync(id);
        }
    }

    public async Task<IEnumerable<TwoFactorCode>> GetTwoFactorCodes()
    {
        var codes = await context.Codes.SelectAsync();
        return codes.Select(code => new TwoFactorCode
        {
            Id = code.Id,
            Description = code.Title,
            Name = code.Name,
            ClientId = code.ClientId,
            SharedKey = code.SharedKey,
            CreatedAt = DateTime.UtcNow
        });
    }
}
