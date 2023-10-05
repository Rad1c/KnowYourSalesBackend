using API.Dtos;
using API.Models;
using API.Models.Validators;
using BLL.IServices;
using DAL.IRepositories;
using ErrorOr;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MODEL.Entities;

namespace API.Controllers;

public class ArticleController : BaseController
{
    private readonly IArticleService _articleService;
    private readonly IArticleRepository _articleRepository;
    private readonly IImageServices _imageService;
    private readonly ISessionService _sessionService;

    public ArticleController(IArticleService article,
        IArticleRepository articleRepository,
        IImageServices imageService,
        ISessionService sessionService)
    {
        _articleService = article;
        _articleRepository = articleRepository;
        _imageService = imageService;
        _sessionService = sessionService;
    }

    [HttpPost("article")]
    public async Task<IActionResult> AddArticle(CreateArticleModel req)
    {
        ValidationResult results = new CreateArticleModelValidator().Validate(req);

        if (!results.IsValid) return ValidationBadRequestResponse(results);

        ErrorOr<Article?> result = await _articleService.CreateArticle(
            _sessionService.Id,
            req.CurrencyName,
            req.ShopIds,
            req.CategoryIds,
            req.Name,
            req.Description,
            req.OldPrice,
            req.NewPrice,
            req.ValidDate);

        return result.Match(
            authResult => Ok(new MessageDto(result.Value!.Id.ToString())),
            errors => Problem(errors));
    }

    [HttpPost("article/add-image")]
    public async Task<IActionResult> AddArticleImage([FromForm] AddArticleImageModel req)
    {
        ValidationResult results = new AddArticleImageModelValidator().Validate(req);

        if (!results.IsValid) return ValidationBadRequestResponse(results);

        ErrorOr<string> saveImageResult = await _imageService.AddArticleImage(req.ArticleId, req.Image);

        if (saveImageResult.IsError) return Problem(saveImageResult.Errors);

        ErrorOr<bool> result = await _articleService.AddArticleImage(req.ArticleId, saveImageResult.Value, req.isThumbnail);

        return result.Match(
        authResult => Ok(new MessageDto(saveImageResult.Value)),
        errors => Problem(errors));
    }

    [HttpDelete("article/{id}")]
    public async Task<IActionResult> DeleteArticle(Guid id)
    {
        ErrorOr<bool> result = await _articleService.DeleteArticle(id);

        if (result.IsError) return Problem(result.Errors);

        return Ok(new MessageDto("article deleted."));
    }

    [HttpPut("article/{id}")]
    public async Task<IActionResult> UpdateArticle(UpdateArticleModel req)
    {
        ValidationResult results = new UpdateArticleModelValidator().Validate(req);

        if (!results.IsValid) ValidationBadRequestResponse(results);

        ErrorOr<Article?> result = await _articleService.UpdateArticle(
            req.Id,
            req.Name,
            req.Description,
            req.NewPrice,
            req.ValidDate);

        return result.Match(
        authResult => Ok(new MessageDto("article updated.")),
        errors => Problem(errors));
    }

    [AllowAnonymous]
    [HttpGet("articles")]
    public async Task<IActionResult> GetArticles(int pageSize, int page, string? name = null, string? cityName = null, string? categoryName = null, Guid? commerceId = null)
    {
        return Ok(await _articleRepository.GetArticlesPaginatedQuery(pageSize, page, name, cityName, categoryName, commerceId));
    }
}

