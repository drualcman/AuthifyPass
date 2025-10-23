using AuthifyPass.API.Core.Interfaces;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.              
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.OperationFilter<AddHeaderOperationFilter>();
});
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();
builder.Services.AddSingleton(sp =>
{
    // Get the address that the app is currently running at
    var server = sp.GetRequiredService<IServer>();
    var addressFeature = server.Features.Get<IServerAddressesFeature>();
    string baseAddress = addressFeature.Addresses.First();
    return new HttpClient { BaseAddress = new Uri(baseAddress) };
});
builder.Services.AddBackendServices(
    dbOptions => builder.Configuration.GetSection(DataBaseOptions.SectionKey).Bind(dbOptions),
    client =>
    {
        var baseAddress = builder.Configuration["ASPNETCORE_URLS"];
        if (!string.IsNullOrEmpty(baseAddress))
        {
            string[] addresses = baseAddress.Split(';', StringSplitOptions.RemoveEmptyEntries);
            client.BaseAddress = new Uri(addresses[0]);
        }
        else
        {
            client.BaseAddress = new Uri("https://localhost/");
        }
    });
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(config =>
    {
        config.AllowAnyHeader();
        config.AllowAnyMethod();
        config.AllowAnyOrigin();
    });
});
builder.Services.AddWebApiDocumentator(options =>
{
    options.ApiName = "AuthifyPass";
    options.Version = "v1";
    options.Description = "Create two factor authentication flow.";
    options.DocsBaseUrl = "swagger";
});

var app = builder.Build();
// Migrate the database at application startup
var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<IWritableDbContext>();
try
{
    await dbContext.MigrateAsync();
    Console.WriteLine("Database migration applied successfully.");
}
catch (Exception ex)
{
    Console.WriteLine($"Error applying database migration: {ex.Message}");
}
scope.Dispose();

//app.UseSwagger();
//app.UseSwaggerUI(options =>
//{
//    options.SwaggerEndpoint("/swagger/v1/swagger.json", "AuthifyPass API V1");
//    options.RoutePrefix = "swagger"; // URL base para Swagger: /swagger
//});                       
app.UseWebApiDocumentatorSessions();
app.UseWebApiDocumentator();
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(AuthifyPass.Views._Imports).Assembly);

app.MapClientEndPoints();
app.MapUserEndPoints();
app.UseCors();

app.UseExceptionHandler(builder => { });

await app.RunAsync();
