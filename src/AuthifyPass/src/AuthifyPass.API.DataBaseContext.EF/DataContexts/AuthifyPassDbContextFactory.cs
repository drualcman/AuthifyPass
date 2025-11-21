using Microsoft.EntityFrameworkCore.Design;

namespace AuthifyPass.API.DataBaseContext.EF.DataContexts;
/// <summary>
/// Add-Migration InitialCreate -p AuthifyPass.API.DataBaseContext.EF -s AuthifyPass.API.DataBaseContext.EF -c AuthifyPassDbContext  -a [connectionString]
/// Update-Database -p AuthifyPass.API.DataBaseContext.EF -s AuthifyPass.API.DataBaseContext.EF -context AuthifyPassDbContext  -a [connectionString]
/// </summary>
internal class AuthifyPassDbContextFactory : IDesignTimeDbContextFactory<AuthifyPassDbContext>
{
    public AuthifyPassDbContext CreateDbContext(string[] args)
    {
        // Validates that a connection string is provided
        if (args == null || args.Length == 0 || string.IsNullOrWhiteSpace(args[0]))
        {
            throw new ArgumentException("A connection string must be provided as the first argument.");
        }
        string connectionstring = args[0];
        IOptions<DataBaseOptions> DBOptions =
          Options.Create(
          new DataBaseOptions
          {
              Writable = connectionstring
          });
        var context = new WritableDbContext(DBOptions);
        return context;
    }
}
