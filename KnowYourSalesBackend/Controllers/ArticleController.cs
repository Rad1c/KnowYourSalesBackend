using API.Dtos;
using API.Models.AddArticleImage;
using API.Models.CreateArticle;
using API.Models.UpdateArticle;
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
    private readonly Supabase.Client _supabaseClient;
    private readonly IConfiguration _configuration;
    private readonly IArticleRepository _articleRepository;


    public ArticleController(IArticleService article, Supabase.Client supabaseClient, IConfiguration configuration, IArticleRepository articleRepository)
    {
        _articleService = article;
        _supabaseClient = supabaseClient;
        _configuration = configuration;
        _articleRepository = articleRepository;
    }

    [HttpPost("article"), AllowAnonymous]
    //[HttpPost("article")]
    public async Task<IActionResult> AddArticle(CreateArticleModel req)
    {
        ValidationResult results = new CreateArticleModelValidator().Validate(req);

        if (!results.IsValid) return ValidationBadRequestResponse(results);

        ErrorOr<Article?> result = await _articleService.CreateArticle(
            req.CommerceId,
            req.CurrencyName,
            req.ShopIds,
            req.CategoryIds,
            req.Name,
            req.Description,
            req.OldPrice,
            req.NewPrice,
            req.ValidDate);

        return result.Match(
            authResult => Ok(new MessageDto(result.Value!.Id.ToString()),
            errors => Problem(errors));
    }

    [HttpPost("article/add-image")]
    public async Task<IActionResult> AddArticleImage([FromForm] AddArticleImageModel req)
    {
        ValidationResult results = new AddArticleImageModelValidator().Validate(req);

        if (!results.IsValid) return ValidationBadRequestResponse(results);

        var lastIndexOf = req.Image.FileName.LastIndexOf(".");
        string exstension = req.Image.FileName[(lastIndexOf + 1)..];
        Guid imgId = Guid.NewGuid();
        string path = $"article-{req.ArticleId}-img-{imgId}.{exstension}";

        ErrorOr<bool> result = await _articleService.AddArticleImage(req.ArticleId, path);

        using var memoryStream = new MemoryStream();
        await req.Image.CopyToAsync(memoryStream);

        await _supabaseClient.Storage.From(_configuration["supabase:productBucket"]).Upload(memoryStream.ToArray(), path);

        return OkResponse<bool>(result, "article image added.");
    }

    [HttpDelete("article/{id}")]
    public async Task<IActionResult> DeleteArticle(Guid id)
    {
        ErrorOr<bool> result = await _articleService.DeleteArticle(id);

        if (result.IsError) return Problem(result.Errors);

        return Ok(new MessageDto("article deleted."));
    }

    [HttpGet("articles")]
    public async Task<IActionResult> GetArticles(int pageSize, int page)
    {
        return Ok(await _articleRepository.GetArticlesPaginatedQuery(pageSize, page));
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
}

