using API.Dtos;
using API.Models.UpdateUser;
using BLL.Enums;
using BLL.Errors;
using BLL.IServices;
using ErrorOr;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using MODEL.QueryModels.User;

namespace API.Controllers;

public class UserController : BaseController
{
    private readonly IUserService _userService;
    private readonly ISessionService _session;

    public UserController(IUserService userService, ISessionService session)
    {
        _userService = userService;
        _session = session;
    }

    [HttpPut("user")]
    public async Task<IActionResult> UpdateUser(UpdateUserModel req)
    {
        ValidationResult results = new UpdateUserModelValidator().Validate(req);

        //TODO: Create response model
        if (!results.IsValid) return BadRequest(results.Errors.Select(x => x.ErrorMessage));

        ErrorOr<bool> updateResult = await _userService.UpdateUser(
            req.Id,                 //TODO: ID from token
            req.FirstName,
            req.LastName,
            req.DateOfBirth,
            Enumeration.GetByCode<SexEnum>(req.Sex));

        //TODO: make this more generic
        return updateResult.Match(
            authResult => Ok(new MessageDto("user updated.")),
            errors => Problem(errors));
    }

    [HttpDelete("user/{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        ErrorOr<bool> result = await _userService.DeleteUser(id);

        if (result.IsError) return Problem(result.Errors);

        return Ok(new MessageDto("user deleted."));
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUser(Guid userId)
    {
        UserQueryModel? user = await _userService.GetUserQuery(userId);

        if (user is null) return Problem(Errors.User.UserNotFound);

        return Ok(user);
    }
}

