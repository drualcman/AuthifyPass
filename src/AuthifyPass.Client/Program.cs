using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddViewsServices(client =>
{
    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
});
var host = builder.Build();
await host.SetDefaultCulture();
await host.RunAsync();
