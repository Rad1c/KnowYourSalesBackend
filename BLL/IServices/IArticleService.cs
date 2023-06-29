using ErrorOr;
using MODEL.Entities;

namespace BLL.IServices;

public interface IArticleService
{
    public Task<ErrorOr<Article?>> CreateArticle(Guid commerceId, string currencyName, List<Guid> shopIds, List<Guid> categoryIds,
        string name, string description, decimal oldPrice, decimal newPrice, string validDate);
    public Task<ErrorOr<Article?>> UpdateArticle(Guid articleId, string? name, string? description,
        decimal? newPrice, string? validDate);
    public Task<ErrorOr<bool>> DeleteArticle(Guid id);
    public Task<ErrorOr<bool>> AddArticleImage(Guid articleId, string path);
}
