using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers;

[AllowAnonymous]
[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorController : BaseController
{
    [Route("error")]
    public IActionResult Error()
    {
        return Problem(title: "Internal server error", statusCode: (int)HttpStatusCode.InternalServerError);
    }
}