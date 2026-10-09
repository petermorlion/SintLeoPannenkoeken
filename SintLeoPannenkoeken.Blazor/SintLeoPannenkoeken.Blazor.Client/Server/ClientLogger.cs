using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SintLeoPannenkoeken.Blazor.Client.Server;

public class ClientLogger : IClientLogger
{
    private readonly HttpClient _httpClient;

    public ClientLogger(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task LogErrorAsync(string component, string message, Exception? exception = null)
    {
        try
        {
            var errorDto = new
            {
                Component = component,
                Message = message,
                Exception = exception?.Message,
                StackTrace = exception?.StackTrace
            };

            await _httpClient.PostAsJsonAsync("api/ClientLogs", errorDto);
        }
        catch
        {
            // If logging fails, at least we tried
        }
    }
}