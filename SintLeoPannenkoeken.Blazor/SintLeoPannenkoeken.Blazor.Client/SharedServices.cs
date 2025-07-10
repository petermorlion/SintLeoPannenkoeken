using Microsoft.AspNetCore.Http;
using MudBlazor.Services;
using SintLeoPannenkoeken.Blazor.Client.Server;

namespace SintLeoPannenkoeken.Blazor.Client
{
    public static class SharedServices
    {
        public static IServiceCollection AddSharedServices(this IServiceCollection services, Uri baseAddress)
        {
            services
                .AddHttpClient<ServerHttpClient>(client => { client.BaseAddress = baseAddress; });

            services.AddMudServices();

            return services;
        }
    }
}