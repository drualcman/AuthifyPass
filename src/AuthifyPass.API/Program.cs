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

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "AuthifyPass API V1");
    options.RoutePrefix = "swagger"; // URL base para Swagger: /swagger
});
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(AuthifyPass.Views._Imports).Assembly);


app.UseClientEndPoints();
app.UseUserEndPoints();
//app.MapGet("/migrate-database", async (IWritableDbContext dbContext) =>
//{
//    try
//    {
//        // Execute the migration using the IWritableDbContext
//        await dbContext.MigrateAsync();
//        return Results.Ok("Database migration applied successfully.");
//    }
//    catch (Exception ex)
//    {
//        // Log or handle the error
//        return Results.Problem($"Database migration failed: {ex.Message}");
//    }
//});
app.UseCors();

app.UseExceptionHandler(builder => { });

await app.RunAsync();
