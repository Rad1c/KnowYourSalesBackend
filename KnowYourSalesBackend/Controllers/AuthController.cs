﻿using API.Dtos;
using API.Models.Login;
using API.Models.RegisterCommerce;
using API.Models.RegisterUser;
using BLL.Enums;
using BLL.Errors;
using BLL.IServices;
using ErrorOr;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using MODEL.Entities;

namespace API.Controllers;

public class AuthController : BaseController
{
    private readonly IUserService _userService;
    private readonly IAuthService _authService;
    private readonly ICommerceService _commerceService;

    public AuthController(IUserService userService, IAuthService authService, ICommerceService commerceService)
    {
        _userService = userService;
        _authService = authService;
        _commerceService = commerceService;
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
            authResult => Ok(new MessageDto("user registerd.")),
            errors => Problem(errors));
    }

    [HttpPost("commerce/register")]
    public async Task<IActionResult> RegisterCommerce(RegisterCommerceModel req)
    {
        ValidationResult results = new RegisterCommerceModelValidator().Validate(req);

        //TODO: Create response model
        if (!results.IsValid) return BadRequest(results.Errors.Select(x => x.ErrorMessage));

        _authService.CreatePasswordHash(req.Password, out byte[] passwordHash, out byte[] passwordSalt);

        ErrorOr<Commerce?> authResult = await _commerceService.RegisterCommerce(
            req.Name,
            passwordHash,
            passwordSalt,
            req.CityId,
            req.Email);

        return authResult.Match(
            authResult => Ok(new MessageDto("commerce registered.")),
            errors => Problem(errors));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginModel req)
    {
        ValidationResult results = new LoginModelValidator().Validate(req);

        //TODO: Create response model
        if (!results.IsValid) return BadRequest(results.Errors.Select(x => x.ErrorMessage));

        ErrorOr<User?> userResult = await _userService.GetUserByEmail(req.Email);

        if (userResult.IsError) return Problem(userResult.Errors);

        if (!_authService.VerifyPasswordHash(req.Password, userResult.Value!.Acc.Password, userResult.Value.Acc.Salt!))
        {
            return Problem(Errors.AuthEr.InvalidCredentials);
        }

        RoleEnum userRole = Enumeration.GetByCode<RoleEnum>(userResult.Value.Acc.Role.Code)!;
        string accessToken = _authService.CreateToken(userResult.Value.Id!, TokenTypeEnum.AccessToken, userRole);
        string refreshToken = _authService.CreateToken(userResult.Value.Id!, TokenTypeEnum.RefreshToken, userRole);

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
            return Problem(Errors.AuthEr.BadToken);
        }
        var claims = _authService.ValidateToken(refreshToken);

        if (claims.Count == 0)
            return Problem(Errors.AuthEr.BadToken);

        if (!claims["type"].Equals(TokenTypeEnum.RefreshToken.Code))
            return Problem(Errors.AuthEr.BadToken);

        string newAccessToken = _authService.CreateToken(Guid.Parse(claims["nameid"]),
            TokenTypeEnum.AccessToken,
            Enumeration.GetByCode<RoleEnum>(claims["role"])!);

        return Ok(newAccessToken);
    }
}

