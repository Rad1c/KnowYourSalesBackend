using DAL.IRepositories;
using Dapper;
using Microsoft.EntityFrameworkCore;
using MODEL;
using MODEL.Entities;

namespace DAL.Repositories;

public class ArticleRepository : Repository, IArticleRepository
{
    private readonly Context _context;
    private readonly QueryContext _queryContext;
    public ArticleRepository(Context context, QueryContext queryContext) : base(context)
    {
        _context = context;
        _queryContext = queryContext;
    }

    public async Task<Article?> GetArticleById(Guid id)
    {
        return await _context.Articles
            .Where(x => x.Id == id && !x.IsDeleted)
            .FirstOrDefaultAsync();
    }

    public async Task<Article?> GetArticleByNameQuery(Guid commerceId, string name)
    {
        string query = "SELECT a.name, a.description, a.old_price, a.new_price, a.sale, a.valid_date, s.\"name\", c.\"name\" FROM article a" +
            "\r\nJOIN article_in_shop ais ON a.id = ais.art_id\r\nJOIN shop s ON ais.id = s.id\r\nJOIN commerce c ON c.id = s.com_id" +
            "\r\nWHERE a.\"name\" = @name AND a.is_deleted = false AND c.id = @commerceId";
        using var connection = _queryContext.CreateConnection();

        var article = await connection.QuerySingleOrDefaultAsync<Article>(query, new { commerceId });

        return article;
    }

    public async Task<Article?> GetArticleByName(Guid commerceId, string name)
    {
        return await _context.Articles
            .Where(x => x.Name == name && !x.IsDeleted)
            .Include(x => x.IdsNavigation.Where(x => x.ComId == commerceId))
            .FirstOrDefaultAsync();
    }
}
