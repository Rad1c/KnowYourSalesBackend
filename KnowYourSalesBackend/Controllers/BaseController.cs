using API.Dtos;
using ErrorOr;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
[ApiController]
public abstract class BaseController : ControllerBase
{
    protected IActionResult Problem(List<Error> errors)
    {
        var firstError = errors[0];
        HttpContext.Items["errors"] = errors;

        var statusCode = firstError.Type switch
        {
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError,
        };

        return Problem(statusCode: statusCode, title: firstError.Description);
    }

    protected IActionResult ValidationBadRequestResponse(ValidationResult validationResult)
    {
        ErrorResponseDto errorResponse = new()
        {
            Message = validationResult.Errors[0].ErrorMessage,
            Errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList()
        };

        return BadRequest(errorResponse);
    }

    protected IActionResult OkResponse<T>(ErrorOr<T> result, string success) where T : notnull
    {
        return result.Match(
            authResult => Ok(new MessageDto(success)),
            errors => Problem(errors));
    }

    protected IActionResult Problem(Error error)
    {
        HttpContext.Items["errors"] = new List<Error>() { error };

        var statusCode = error.Type switch
        {
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError,
        };

        return Problem(statusCode: statusCode, title: error.Description);
    }
}