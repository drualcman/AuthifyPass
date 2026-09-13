using AuthifyPass.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddViewsServices(client =>
{
    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
});
WebAssemblyHost host = builder.Build();
await host.SetDefaultCulture();
await host.RunAsync();
