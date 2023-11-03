using BLL.Helper;
using MODEL.Entities;
using MODEL.QueryModels.Article;

namespace DAL.IRepositories;

public interface IArticleRepository : IRepository<Article>
{
    public Task<Article?> GetArticleByNameQuery(Guid id, string name);
    public Task<Article?> GetArticleByName(Guid id, string name);
    public Task<Article?> GetArticleWithImages(Guid id);
    public Task<PaginatedList<ArticleQueryModel>> GetArticlesPaginatedQuery(int pageSize, int page, string? name = null, Guid? cityId = null, Guid? categoryId = null, Guid? commerceId = null);
    public Task<ArticleDetailsQueryModel> GetArticleDetails(Guid id);
}
