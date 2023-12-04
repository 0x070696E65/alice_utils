using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using aLice_utils.Client;
using aLice_utils.Client.Services;
using BlazorStrap;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddSingleton<WebSocketService>();

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddBlazorStrap();

await builder.Build().RunAsync();

