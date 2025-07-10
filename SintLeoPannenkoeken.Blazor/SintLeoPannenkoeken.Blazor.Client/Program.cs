using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using MudBlazor.Translations;
using SintLeoPannenkoeken.Blazor.Client;
using SintLeoPannenkoeken.Blazor.Client.Server;
using System.Globalization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

var culture = new CultureInfo("nl-BE");
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();

builder.Services.AddHttpClient<IServerData, ServerHttpClient>(client => { client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress); });
builder.Services.AddSharedServices();

builder.Services.AddMudServices();
builder.Services.AddMudTranslations();

await builder.Build().RunAsync();
