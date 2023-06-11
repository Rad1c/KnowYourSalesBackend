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

        if (!results.IsValid) return ValidationBadRequestResponse(results);

        ErrorOr<bool> updateResult = await _userService.UpdateUser(
            req.Id,                 //TODO: ID from token
            req.FirstName,
            req.LastName,
            req.DateOfBirth,
            Enumeration.GetByCode<SexEnum>(req.Sex));

        return OkResponse<bool>(updateResult, "user updated.");
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

        if (!results.IsValid) return ValidationBadRequestResponse(results);

        ErrorOr<bool> result = await _userService.AddUserImpression(req.UserId, req.Impression);

        return OkResponse<bool>(result, "impression added.");
    }

    [HttpPut("user/favoriteCommerce/add")]
    public async Task<IActionResult> AddFavoriteCommerce(AddFavoriteCommerceModel req)
    {
        ValidationResult results = new AddFavoriteCommerceModelValidator().Validate(req);

        if (!results.IsValid) return ValidationBadRequestResponse(results);

        ErrorOr<bool> result = await _userService.AddFavoriteCommerce(req.UserId, req.CommerceId);

        return OkResponse<bool>(result, "commerce added in favorites.");
    }

    [HttpPut("user/favoriteCommerce/remove")]
    public async Task<IActionResult> RemoveFromFavoriteCommerces(RemoveFromFavoriteCommercesModel req)
    {
        ValidationResult results = new RemoveFromFavoriteCommercesModelValidator().Validate(req);

        if (!results.IsValid) return ValidationBadRequestResponse(results);

        ErrorOr<bool> result = await _userService.RemoveCommerceFromFavorites(req.Id, req.CommerceId);

        return OkResponse<bool>(result, "commerce removed from favorites.");
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

        if (!results.IsValid) return ValidationBadRequestResponse(results);

        ErrorOr<bool> result = await _userService.AddFavoriteArticle(req.Id, req.ArticleId);

        return OkResponse<bool>(result, "article added in favorites.");
    }
}

