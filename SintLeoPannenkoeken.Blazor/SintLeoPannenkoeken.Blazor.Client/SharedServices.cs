using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using SintLeoPannenkoeken.Blazor.Client.Server;

namespace SintLeoPannenkoeken.Blazor.Client
{
    public static class SharedServices
    {
        public static IServiceCollection AddSharedServices(this IServiceCollection services, Uri baseAddress)
        {
            services.AddScoped(sp => new HttpClient { BaseAddress = baseAddress });
            services.AddTransient<ServerHttpClient>();

            services.AddMudServices();

            return services;
        }
    }
}