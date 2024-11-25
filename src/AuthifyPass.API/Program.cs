using AuthifyPass.API.Components;
using AuthifyPass.API.Core.DTOs;
using AuthifyPass.API.Core.Interfaces.UseCases.RegisterClient;
using AuthifyPass.Entities.DTOs;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.              
builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();
builder.Services.AddServices();

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


app.MapGet("/ping", () =>
{
    return Results.Ok("Pong!");
})
.WithName("Ping")
.WithTags("Test Endpoints");

app.MapPost("/create-client", async (RegisterClientDto data, 
    IRegisterClientController controller) => Results.Ok(await controller.CreateClientAsync(data)))
.WithName("Create Client")
.WithTags("Create Client")
.Produces<RegisterClientResultDto>(StatusCodes.Status200OK);

await app.RunAsync();