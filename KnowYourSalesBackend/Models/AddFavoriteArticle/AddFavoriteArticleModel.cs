namespace API.Models;

public record AddFavoriteArticleModel
{
    public Guid ArticleId { get; init; }
}
