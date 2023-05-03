using API.Dtos;
using API.Models.CreateArticle;
using BLL.IServices;
using DAL.IRepositories;
using ErrorOr;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using MODEL.Entities;

namespace API.Controllers;
public class ArticleController : BaseController
{
    private readonly IArticleService _articleService;
    private readonly IArticleRepository _articleRepository;

    public ArticleController(IArticleService article, IArticleRepository articleRepository)
    {
        _articleService = article;
        _articleRepository = articleRepository;
    }

    [HttpPost("article")]
    public async Task<IActionResult> AddArticle(CreateArticleModel req)
    {
        ValidationResult results = new CreateArticleModelValidator().Validate(req);

        if (!results.IsValid) return BadRequest(results.Errors.Select(x => x.ErrorMessage));

        ErrorOr<Article?> result = await _articleService.CreateArticle(req.CommerceId, req.ShopIds, req.CategoryIds, req.Name, req.Description, req.OldPrice, req.NewPrice, req.ValidDate);

        return result.Match(
            authResult => Ok(new MessageDto("article added.")),
            errors => Problem(errors));
    }

    [HttpDelete("article/{id}")]
    public async Task<IActionResult> DeleteArticle(Guid id)
    {
        ErrorOr<bool> result = await _articleService.DeleteArticle(id);

        if (result.IsError) return Problem(result.Errors);

        return Ok(new MessageDto("article deleted."));
    }
}

