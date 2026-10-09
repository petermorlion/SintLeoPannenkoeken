using System;
using System.Threading.Tasks;

namespace SintLeoPannenkoeken.Blazor.Client.Server;

public interface IClientLogger
{
    Task LogErrorAsync(string component, string message, Exception? exception = null);
}