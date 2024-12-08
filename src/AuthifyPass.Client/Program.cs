using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddViewsServices(client =>
{
    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
});
await builder.Build().RunAsync();
