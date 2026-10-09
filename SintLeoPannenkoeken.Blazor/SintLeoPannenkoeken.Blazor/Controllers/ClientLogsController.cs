using Microsoft.AspNetCore.Mvc;
using SintLeoPannenkoeken.Blazor.Models;

namespace SintLeoPannenkoeken.Blazor.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientLogsController : ControllerBase
{
    private readonly ILogger<ClientLogsController> _logger;

    public ClientLogsController(ILogger<ClientLogsController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public IActionResult LogError([FromBody] ClientErrorDto error)
    {
        _logger.LogError(error.Exception, "Client error in {Component}: {Message}",
            error.Component, error.Message);
        return Ok();
    }
}