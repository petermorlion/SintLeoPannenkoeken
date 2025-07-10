using MudBlazor.Services;

namespace SintLeoPannenkoeken.Blazor.Client
{
    public static class SharedServices
    {
        public static IServiceCollection AddSharedServices(this IServiceCollection services)
        {
            services.AddMudServices();

            return services;
        }
    }
}