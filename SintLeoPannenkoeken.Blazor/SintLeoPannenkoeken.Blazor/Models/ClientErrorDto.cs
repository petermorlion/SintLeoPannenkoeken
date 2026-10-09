namespace SintLeoPannenkoeken.Blazor.Models;

public class ClientErrorDto
{
    public string Component { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? Exception { get; set; }
    public string? StackTrace { get; set; }
}