using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers;

[AllowAnonymous]
[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorController : BaseController
{
    private readonly ILogger<ErrorController> _logger;

    public ErrorController(ILogger<ErrorController> logger)
    {
        _logger = logger;
    }

    [Route("error")]
    public IActionResult Error()
    {
        Exception? exc = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

        if (exc is not null) _logger.LogError(exc.Message);

        return Problem(title: "Internal server error", statusCode: (int)HttpStatusCode.InternalServerError);
    }
}