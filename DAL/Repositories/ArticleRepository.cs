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

    public async Task<List<ArticleQueryModel>> GetArticlesPaginatedQuery(int pageSize, int page, string? name, string? cityName = null, string? categoryName = null, Guid? commerceId = null)
    {
        var queryBuilder = new StringBuilder("SELECT a.name, a.old_price AS \"oldPrice\", a.new_price AS \"newPrice\", a.sale, a.created, a.valid_date AS \"validDate\", p.pic AS \"picture\", c.logo FROM article a ")
            .Append("JOIN article_in_shop ais ON a.id = ais.art_id JOIN shop s ON ais.id = s.id JOIN commerce c ON c.id = s.com_id ")
            .Append("JOIN city ON city.id = s.cit_id JOIN picture p ON a.id = p.art_id JOIN article_in_category aic ON aic.art_id = a.id ")
            .Append("JOIN category cat ON cat.id = aic.id ")
            .Append("WHERE p.is_thumbnail = true");

        if (cityName != null)
        {
            queryBuilder.Append(" AND city.name = @cityName");
        }
        if (categoryName != null)
        {
            queryBuilder.Append(" AND cat.name = @categoryName");
        }
        if (name is not null)
        {
            queryBuilder.Append(" AND a.name = @name");
        }
        if (commerceId != null)
        {
            queryBuilder.Append(" AND c.id = @commerceId");
        }

        queryBuilder.Append(" ORDER BY a.created DESC")
            .Append(" OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY");

        using var connection = _queryContext.CreateConnection();

        var offset = (page - 1) * pageSize;

        var article = await connection.QueryAsync<ArticleQueryModel>(queryBuilder.ToString(), new { cityName, categoryName, name, offset, pageSize, commerceId });

        return article.ToList();
    }

    public async Task<Article?> GetArticleWithImages(Guid id)
    {
        return await _context.Articles.Include(p => p.Pictures).FirstOrDefaultAsync(p => p.Id == id);
    }
}
