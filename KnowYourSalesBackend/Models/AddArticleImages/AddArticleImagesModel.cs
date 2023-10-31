namespace API.Models;

public record AddArticleImagesModel
{
    public Guid ArticleId { get; set; }
    public List<ImageModel> Images { get; init; } = new();
}
