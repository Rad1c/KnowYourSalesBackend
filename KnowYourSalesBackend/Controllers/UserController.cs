using API.Dtos;
using API.Models.AddFavoriteArticle;
using API.Models.AddFavoriteCommerce;
using API.Models.RemoveFavoriteCommerce;
using API.Models.UpdateUser;
using API.Models.UserImpression;
using BLL.Enums;
using BLL.Errors;
using BLL.IServices;
using DAL.IRepositories;
using ErrorOr;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using MODEL.QueryModels.User;

namespace API.Controllers;

public class UserController : BaseController
{
    private readonly IUserService _userService;
    private readonly ISessionService _session;
    private readonly IUserRepository _userRepository;

    public UserController(IUserService userService, ISessionService session, IUserRepository userRepository)
    {
        _userService = userService;
        _session = session;
        _userRepository = userRepository;
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

        if (user is null) return Problem(Errors.UserEr.UserNotFound);

        return Ok(user);
    }

    [HttpPost("user/impressions")]
    public async Task<IActionResult> AddUserImpression(UserImpressionModel req)
    {
        ValidationResult results = new UserImpressionModelValidator().Validate(req);

        //TODO: Create response model
        if (!results.IsValid) return BadRequest(results.Errors.Select(x => x.ErrorMessage));

        ErrorOr<bool> result = await _userService.AddUserImpression(req.UserId, req.Impression);

        return result.Match(
            authResult => Ok(new MessageDto("impression added.")),
            errors => Problem(errors));
    }

    [HttpPut("user/favoriteCommerce/add")]
    public async Task<IActionResult> AddFavoriteCommerce(AddFavoriteCommerceModel req)
    {
        ValidationResult results = new AddFavoriteCommerceModelValidator().Validate(req);

        //TODO: Create response model
        if (!results.IsValid) return BadRequest(results.Errors.Select(x => x.ErrorMessage));

        ErrorOr<bool> result = await _userService.AddFavoriteCommerce(req.UserId, req.CommerceId);

        return result.Match(
            authResult => Ok(new MessageDto("commerce added in favorites.")),
            errors => Problem(errors));
    }

    [HttpPut("user/favoriteCommerce/remove")]
    public async Task<IActionResult> RemoveFromFavoriteCommerces(RemoveFromFavoriteCommercesModel req)
    {
        ValidationResult results = new RemoveFromFavoriteCommercesModelValidator().Validate(req);

        //TODO: Create response model
        if (!results.IsValid) return BadRequest(results.Errors.Select(x => x.ErrorMessage));

        ErrorOr<bool> result = await _userService.RemoveCommerceFromFavorites(req.Id, req.CommerceId);

        return result.Match(
            authResult => Ok(new MessageDto("commerce removed from favorites.")),
            errors => Problem(errors));
    }

    [HttpGet("user/favoriteCommerce")]
    public async Task<IActionResult> GetFavoritesCommerces(Guid userId) //from token
    {
        return Ok(await _userRepository.GetFavoriteCommercesQuery(userId));
    }

    [HttpGet("/user/impressions")]
    public async Task<IActionResult> GetUserImpressions()
    {
        return Ok(await _userRepository.GetImpressions());
    }

    [HttpPut("user/favoriteArticle/add")]
    public async Task<IActionResult> AddFavoriteArticle(AddFavoriteArticleModel req)
    {
        ValidationResult results = new AddFavoriteArticleModelValidator().Validate(req);

        //TODO: Create response model
        if (!results.IsValid) return BadRequest(results.Errors.Select(x => x.ErrorMessage));

        ErrorOr<bool> result = await _userService.AddFavoriteArticle(req.Id, req.ArticleId);

        return result.Match(
            authResult => Ok(new MessageDto("article added in favorites.")),
            errors => Problem(errors));
    }
}

