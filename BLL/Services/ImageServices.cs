using BLL.Enums;
using BLL.IServices;
using DAL.IRepositories;
using ErrorOr;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MODEL.Entities;

namespace BLL.Services;

public class ImageServices : IImageServices
{
    private readonly Supabase.Client _supabaseClient;
    private readonly IConfiguration _configuration;
    private readonly IArticleRepository _articleRepository;

    public ImageServices(Supabase.Client supabaseClient, IConfiguration configuration, IArticleRepository articleRepository)
    {
        _supabaseClient = supabaseClient;
        _configuration = configuration;
        _articleRepository = articleRepository;
    }

    public async Task<ErrorOr<string>> AddArticleImage(Guid articleId, IFormFile img)
    {
        Article? article = await _articleRepository.GetById<Article>(articleId);

        if (article is null) return Errors.Errors.Article.ArticleNotFound;

        var lastIndexOf = img.FileName.LastIndexOf(".");
        string exstension = img.FileName[(lastIndexOf + 1)..];

        if (!ImageExstensionEnum.ContainsCode<ImageExstensionEnum>(exstension)) return Errors.Errors.Images.ImgExstensionNotSuported;

        Guid imgId = Guid.NewGuid();
        string path = $"article-{articleId}-img-{imgId}.{exstension}";

        using var memoryStream = new MemoryStream();
        await img.CopyToAsync(memoryStream);

        await _supabaseClient.Storage.From(_configuration["supabase:productBucket"]).Upload(memoryStream.ToArray(), path);

        return _supabaseClient.Storage.From(_configuration["supabase:productBucket"]).GetPublicUrl(path);
    }
}
