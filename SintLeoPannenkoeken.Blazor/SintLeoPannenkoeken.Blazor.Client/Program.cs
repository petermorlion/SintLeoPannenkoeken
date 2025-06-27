using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using SintLeoPannenkoeken.Blazor.Client;
using SintLeoPannenkoeken.Blazor.Client.Server;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();

builder.Services.AddMudServices();

builder.Services.AddHttpClient<IServerHttpClient, ServerHttpClient>();

await builder.Build().RunAsync();
