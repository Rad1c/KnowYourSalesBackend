using BLL.Enums;
using BLL.IServices;
using DAL.IRepositories;
using ErrorOr;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

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
        MODEL.Entities.Article? article = await _articleRepository.GetById<MODEL.Entities.Article>(articleId);

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

    public async Task<ErrorOr<string>> AddArticleImage(Guid articleId, string base64Image)
    {
        base64Image = SanitazeBase64String(base64Image);

        string exstension = GetImageExtension(base64Image).Code;

        byte[] imageBytes = Convert.FromBase64String(base64Image);

        if (!ImageExstensionEnum.ContainsCode<ImageExstensionEnum>(exstension)) return Errors.Errors.Images.ImgExstensionNotSuported;

        Guid imgId = Guid.NewGuid();
        string path = $"article-{articleId}-img-{imgId}.{exstension}";

        string bucketName = _configuration["supabase:productBucket"];

        await _supabaseClient.Storage.From(bucketName).Upload(imageBytes, path);

        return _supabaseClient.Storage.From(bucketName).GetPublicUrl(path);
    }
    public async Task<ErrorOr<string>> AddCommerceImage(Guid commerceId, string base64Img)
    {
        base64Img = SanitazeBase64String(base64Img);

        string exstension = GetImageExtension(base64Img).Code;

        byte[] imageBytes = Convert.FromBase64String(base64Img);

        if (!ImageExstensionEnum.ContainsCode<ImageExstensionEnum>(exstension)) return Errors.Errors.Images.ImgExstensionNotSuported;

        Guid imgId = Guid.NewGuid();
        string path = $"commerce-{commerceId}-img-{imgId}.{exstension}";

        string bucketName = _configuration["supabase:productBucket"];

        await _supabaseClient.Storage.From(bucketName).Upload(imageBytes, path);

        return _supabaseClient.Storage.From(bucketName).GetPublicUrl(path);
    }

    private static ImageExstensionEnum GetImageExtension(string base64Image)
    {
        base64Image = SanitazeBase64String(base64Image);

        // Decode the base64 string to bytes
        byte[] imageBytes = Convert.FromBase64String(base64Image);

        // Determine the file extension based on the magic bytes
        if (imageBytes.Length < 4)
        {
            return ImageExstensionEnum.Unsupported;
        }

        if (imageBytes[0] == 0xFF && imageBytes[1] == 0xD8 && imageBytes[2] == 0xFF)
        {
            return ImageExstensionEnum.Jpeg; // JPEG format
        }
        else if (imageBytes[0] == 0x89 && imageBytes[1] == 0x50 && imageBytes[2] == 0x4E && imageBytes[3] == 0x47)
        {
            return ImageExstensionEnum.Png; // PNG format
        }
        else if (imageBytes[0] == 0x47 && imageBytes[1] == 0x49 && imageBytes[2] == 0x46)
        {
            return ImageExstensionEnum.Gif; // GIF format
        }
        else if (imageBytes.Length >= 4 && imageBytes[0] == 0x3C && imageBytes[1] == 0x3F && imageBytes[2] == 0x78 && imageBytes[3] == 0x6D)
        {
            return ImageExstensionEnum.Svg; //SVG format
        }
        else if (imageBytes.Length >= 2 && imageBytes[0] == 0xFF && imageBytes[1] == 0xD8)
        {
            return ImageExstensionEnum.Jpg; //JPG format
        }

        return ImageExstensionEnum.Unsupported; // Unknown format or unable to determine the extension
    }

    private static string SanitazeBase64String(string base64Image)
    {
        // Remove the data URI prefix if it exists (e.g., "data:image/jpeg;base64,")
        if (base64Image.Contains(","))
        {
            return base64Image.Split(',')[1];
        }

        return base64Image;
    }
}