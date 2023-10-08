using API.Dtos;
using API.Models;
using API.Models.Validators;
using BLL.Enums;
using BLL.Errors;
using BLL.IServices;
using ErrorOr;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MODEL.Entities;

namespace API.Controllers;

[AllowAnonymous]
public class AuthController : BaseController
{
    private readonly IUserService _userService;
    private readonly IAuthService _authService;
    private readonly ICommerceService _commerceService;
    private readonly IEmailService _mailService;

    public AuthController(IUserService userService,
        IAuthService authService,
        ICommerceService commerceService,
        IEmailService mailService)
    {
        _userService = userService;
        _authService = authService;
        _commerceService = commerceService;
        _mailService = mailService;
    }

    [HttpPost("user/register")]
    public async Task<IActionResult> RegisterUser(RegisterUserModel req)
    {
        ValidationResult results = new RegisterUserModelValidator().Validate(req);

        if (!results.IsValid) return ValidationBadRequestResponse(results);

        _authService.CreatePasswordHash(req.Password, out byte[] passwordHash, out byte[] passwordSalt);

        string emailVerificationCode = _authService.GenerateEmailVerificationCode();

        ErrorOr<User?> authResult = await _userService.RegisterUser(
            req.FirstName,
            req.LastName,
            req.DateOfBirth,
            passwordHash,
            passwordSalt,
            req.Sex,
            req.Email,
            emailVerificationCode);

        if (!authResult.IsError)
        {
            await _mailService.SendVerifyUserAccountEmail(req.Email, req.FirstName, req.LastName, emailVerificationCode);
        }

        return authResult.Match(
            authResult => Ok(new MessageDto("user registerd.")),
            errors => Problem(errors));
    }

    [HttpPost("commerce/register")]
    public async Task<IActionResult> RegisterCommerce(RegisterCommerceModel req)
    {
        ValidationResult results = new RegisterCommerceModelValidator().Validate(req);

        if (!results.IsValid) return ValidationBadRequestResponse(results);

        _authService.CreatePasswordHash(req.Password, out byte[] passwordHash, out byte[] passwordSalt);
        string emailVerificationCode = _authService.GenerateEmailVerificationCode();

        ErrorOr<Commerce?> authResult = await _commerceService.RegisterCommerce(
            req.Name,
            passwordHash,
            passwordSalt,
            req.CityId,
            req.Email,
            emailVerificationCode);

        if (!authResult.IsError)
        {
            await _mailService.SendVerifyCommerceAccountEmail(req.Email, req.Name, emailVerificationCode);
        }

        return authResult.Match(
            authResult => Ok(new MessageDto("commerce registered.")),
            errors => Problem(errors));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginModel req)
    {
        ValidationResult results = new LoginModelValidator().Validate(req);

        if (!results.IsValid) return ValidationBadRequestResponse(results);

        ErrorOr<Account?> userResult = await _authService.GetAccountByEmail(req.Email);

        if (userResult.IsError) return Problem(userResult.Errors);

        if (userResult.Value != null && !userResult.Value.IsEmailVerified) return Forbid();

        if (!_authService.VerifyPasswordHash(req.Password, userResult.Value!.Password, userResult.Value.Salt!))
        {
            return Problem(Errors.Auth.InvalidCredentials);
        }

        RoleEnum userRole = Enumeration.GetByCode<RoleEnum>(userResult.Value.Role.Code)!;
        Guid id;
        Guid accountId = userResult.Value.Id;
        if (userResult.Value.Role.Code == RoleEnum.User.Code)
        {
            User? user = await _userService.GetUserByAccountId(accountId);
            id = user!.Id;
        }
        else
        {
            Commerce? commerce = await _commerceService.GetCommerceByAccountId(accountId);
            id = commerce!.Id;
        }
        string accessToken = _authService.CreateToken(id, accountId, TokenTypeEnum.AccessToken, userRole);
        string refreshToken = _authService.CreateToken(id, accountId, TokenTypeEnum.RefreshToken, userRole);

        TokenDto token = new()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
        };

        return Ok(token);
    }

    [HttpPost("refresh-token")]
    public IActionResult RefreshToken(string refreshToken)
    {
        if (String.IsNullOrEmpty(refreshToken))
        {
            return Problem(Errors.Auth.BadToken);
        }
        var claims = _authService.ValidateToken(refreshToken);

        if (claims.Count == 0)
            return Problem(Errors.Auth.BadToken);

        if (!claims["type"].Equals(TokenTypeEnum.RefreshToken.Code))
            return Problem(Errors.Auth.BadToken);

        string newAccessToken = _authService.CreateToken(
            Guid.Parse(claims["nameid"]),
            Guid.Parse(claims["accountId"]),
            TokenTypeEnum.AccessToken,
            Enumeration.GetByCode<RoleEnum>(claims["role"])!);

        return Ok(newAccessToken);
    }

    [AllowAnonymous]
    [HttpPost("verify-account")]
    public async Task<IActionResult> VerifyAccount(VerifyAccountModel model)
    {
        bool isSuccess = await _authService.VerifyAccount(model.Code);

        return isSuccess ? Ok(new MessageDto("account verified."))
            : Forbid();
    }

    [AllowAnonymous]
    [HttpGet("test")]
    public async Task<IActionResult> Test(string text)
    {
        string emailVerificationCode = _authService.GenerateEmailVerificationCode();
        await _mailService.SendVerifyUserAccountEmail("radicaleksandar4@gmail.com", "Aleksandar", "Radic", emailVerificationCode);
        return Ok("ok");
    }
}

