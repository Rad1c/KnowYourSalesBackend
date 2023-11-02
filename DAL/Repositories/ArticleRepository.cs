using BLL.Helper;
using DAL.IRepositories;
using Dapper;
using Microsoft.EntityFrameworkCore;
using MODEL;
using MODEL.Entities;
using MODEL.QueryModels.ReferenteData;
using System.Text;

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

    public async Task<Article?> GetArticleByNameQuery(Guid commerceId, string name)
    {
        string query = "SELECT a.name, a.description, a.old_price, a.new_price, a.sale, a.valid_date, s.name, c.name FROM article a" +
            "\r\nJOIN article_in_shop ais ON a.id = ais.art_id\r\nJOIN shop s ON ais.id = s.id\r\nJOIN commerce c ON c.id = s.com_id" +
            "\r\nWHERE a.name = @name AND a.is_deleted = false AND c.id = @commerceId";
        using var connection = _queryContext.CreateConnection();

        var article = await connection.QuerySingleOrDefaultAsync<Article>(query, new { name, commerceId });

        return article;
    }

    public async Task<Article?> GetArticleByName(Guid commerceId, string name)
    {
        return await _context.Articles
            .Where(x => x.Name == name && !x.IsDeleted)
            .Include(x => x.Shops.Where(x => x.CommerceId == commerceId))
            .FirstOrDefaultAsync();
    }

    public async Task<PaginatedList<ArticleQueryModel>> GetArticlesPaginatedQuery(int pageSize, int page, string? name, Guid? cityId = null, Guid? categoryId = null, Guid? commerceId = null)
    {
        var queryBuilder = new StringBuilder($"SELECT DISTINCT \"articleId\" as \"id\", \"commerceId\", \"categoryId\", \"cityId\", \"articleName\" as \"name\", \"oldPrice\", \"newPrice\", sale, created,  \"validDate\", pic as \"picture\", logo FROM mv_articles where true");

        if (cityId != null)
        {
            queryBuilder.Append(" AND \"cityId\" = @cityId");
        }
        if (categoryId != null)
        {
            queryBuilder.Append(" AND \"categoryId\" = @categoryId");
        }
        if (name is not null)
        {
            queryBuilder.Append(" AND name = @name");
        }
        if (commerceId != null)
        {
            queryBuilder.Append(" AND \"commerceId\" = @commerceId");
        }

        using var connection = _queryContext.CreateConnection();

        int total = await connection.QuerySingleAsync<int>($"SELECT count(*) from({queryBuilder}) as \"result\"", new { cityId, categoryId, name, commerceId });

        queryBuilder.Append(" ORDER BY created DESC")
            .Append(" OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY");

        var offset = (page - 1) * pageSize;

        var articles = await connection.QueryAsync<ArticleQueryModel>(queryBuilder.ToString(), new { cityId, categoryId, name, offset, pageSize, commerceId });

        return new PaginatedList<ArticleQueryModel>(articles, page, pageSize, total);
    }

    public async Task<Article?> GetArticleWithImages(Guid id)
    {
        return await _context.Articles.Include(p => p.Pictures).FirstOrDefaultAsync(p => p.Id == id);
    }
}
