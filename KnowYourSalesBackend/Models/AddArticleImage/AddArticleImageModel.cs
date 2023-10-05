namespace API.Models;

public record AddArticleImageModel
{
    public Guid ArticleId { get; init; }
    public IFormFile Image { get; init; } = null!;
    public bool isThumbnail { get; init; }
}
