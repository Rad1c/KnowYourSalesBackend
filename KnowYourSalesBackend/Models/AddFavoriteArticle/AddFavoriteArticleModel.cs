namespace API.Models.AddFavoriteArticle;

public record AddFavoriteArticleModel
{
    public Guid Id { get; init; } //TODO: from token
    public Guid ArticleId { get; init; }
}
