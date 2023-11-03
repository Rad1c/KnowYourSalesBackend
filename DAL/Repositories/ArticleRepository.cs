using BLL.Helper;
using DAL.IRepositories;
using Dapper;
using Microsoft.EntityFrameworkCore;
using MODEL;
using MODEL.Entities;
using MODEL.QueryModels.Article;
using MODEL.QueryModels.Common;
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
            name = name.Trim();
            name = $"%{name}%";

            queryBuilder.Append(" AND  LOWER(\"articleName\") LIKE LOWER(@Name)");
        }
        if (commerceId != null)
        {
            queryBuilder.Append(" AND \"commerceId\" = @commerceId");
        }

        using var connection = _queryContext.CreateConnection();

        int total = await connection.QuerySingleAsync<int>($"SELECT count(*) from({queryBuilder}) as \"result\"", new { cityId, categoryId, Name = name, commerceId });

        queryBuilder.Append(" ORDER BY created DESC")
            .Append(" OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY");

        var offset = (page - 1) * pageSize;

        var articles = await connection.QueryAsync<ArticleQueryModel>(queryBuilder.ToString(), new { cityId, categoryId, Name = name, offset, pageSize, commerceId });

        articles = articles.GroupBy(x => x.Id).Select(x => x.First());

        return new PaginatedList<ArticleQueryModel>(articles, page, pageSize, total);
    }

    public async Task<Article?> GetArticleWithImages(Guid id)
    {
        return await _context.Articles.Include(p => p.Pictures).FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<ArticleDetailsQueryModel> GetArticleDetails(Guid id)
    {
        string query = "select distinct \"articleId\", \"articleName\" as \"name\", description, \"oldPrice\", \"newPrice\" , sale, \"validDate\" , logo as \"commerceLogo\", \"categoryId\" , \"commerceId\"  from mv_articles where \"articleId\" = @id";
        using var connection = _queryContext.CreateConnection();

        var article = await connection.QuerySingleOrDefaultAsync<ArticleDetailsQueryModel>(query, new { id });

        query = "select pic from picture p where p.art_id  = @id";
        var articleImages = await connection.QueryAsync<string>(query, new { id });

        query = "select g.longitude, g.latitude, g.address from  article a join article_in_shop ais on ais.art_id  = a.id join shop s on s.id  =  ais.id join geopoint g on g.id = s.geo_id where a.id = @id and a.is_deleted = false and s.is_deleted  = false";
        var geoLocations = await connection.QueryAsync<GeoQueryModel>(query, new { id });

        if (article is not null)
        {
            if (geoLocations is not null)
            {
                article.GeoLocations = geoLocations;
            }

            if (articleImages is not null)
            {
                article.Images = articleImages;
            }
        }

        return article!;
    }
}
