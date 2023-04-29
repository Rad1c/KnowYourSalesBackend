using API.Models.RegisterUser;
using BLL.IServices;
using ErrorOr;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using MODEL.Entities;

namespace API.Controllers
{
    public class AuthController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;

        public AuthController(IUserService userService, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        [HttpPost("user/register")]
        public async Task<IActionResult> RegisterUser(RegisterUserModel req)
        {
            ValidationResult results = new RegisterUserModelValidator().Validate(req);

            //TODO: Create response model
            if (!results.IsValid) return BadRequest(results.Errors.Select(x => x.ErrorMessage));

            _authService.CreatePasswordHash(req.Password, out byte[] passwordHash, out byte[] passwordSalt);

            ErrorOr<User?> authResult = await _userService.RegisterUser(
                req.FirstName,
                req.LastName,
                req.DateOfBirth,
                passwordHash,
                passwordSalt,
                req.Sex,
                req.Email);

            return authResult.Match(
                authResult => Ok("user registerd."),
                errors => Problem(errors));
        }
    }
}
