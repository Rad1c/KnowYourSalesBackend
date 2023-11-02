using BLL.Helper;
using BLL.IServices;
using DAL.IRepositories;
using ErrorOr;
using Microsoft.Extensions.Configuration;
using MODEL.Entities;
using Article = MODEL.Entities.Article;
using Commerce = MODEL.Entities.Commerce;

namespace BLL.Services;

public class ArticleService : IArticleService
{
    private readonly IArticleRepository _articleRepository;
    private readonly IShopRepository _shopRepository;
    private readonly IConfiguration _configuration;

    public ArticleService(IArticleRepository articleRepository, IShopRepository shopRepository, IConfiguration configuration)
    {
        _articleRepository = articleRepository;
        _shopRepository = shopRepository;
        _configuration = configuration;
    }

    public async Task<ErrorOr<bool>> AddArticleImage(Guid articleId, string path, bool isThumbnail)
    {
        MODEL.Entities.Article? article = await _articleRepository.GetArticleWithImages(articleId);

        if (article is null) return Errors.Errors.Article.ArticleNotFound;

        if (article.Pictures.Count > int.Parse(_configuration["maxImagesPerArticle"])) return Errors.Errors.Article.ToMuchImagesPerProduct;

        Picture newPicture = new()
        {
            ArticleId = article.Id,
            PicUrl = path,
            IsThumbnail = isThumbnail
        };

        _articleRepository.Save<Picture>(newPicture);

        return true;
    }
    public ErrorOr<bool> AddArticleImage(MODEL.Entities.Article article, string path, bool isThumbnail)
    {
        if (article is null) return Errors.Errors.Article.ArticleNotFound;

        if (article.Pictures.Count > int.Parse(_configuration["maxImagesPerArticle"])) return Errors.Errors.Article.ToMuchImagesPerProduct;

        Picture newPicture = new()
        {
            ArticleId = article.Id,
            PicUrl = path,
            IsThumbnail = isThumbnail
        };

        _articleRepository.Save<Picture>(newPicture);

        return true;
    }

    public async Task<ErrorOr<MODEL.Entities.Article?>> CreateArticle(Guid commerceId, string currencyName, List<Guid> shopIds, List<Guid> categoryIds, string name, string description, decimal oldPrice, decimal newPrice, string validDate)
    {
        Commerce? commerce = await _shopRepository.GetCommerceWithShops(commerceId);

        Currency? currency = await _shopRepository.GetCurrencyByName(currencyName);

        if (currency is null) return Errors.Errors.Article.CurrencyNotFound;

        foreach (var item in shopIds)
        {
            if (!commerce.Shops.Any(x => x.Id == item))
            {
                return Errors.Errors.Shop.ShopNotFound;
            }
        }

        List<Category> categories = await _shopRepository.GetCategories();
        if (categories is null) return Errors.Errors.Shop.CategoryNotFound;

        foreach (var item in categoryIds)
        {

            if (!categories.Any(x => x.Id == item))
            {
                return Errors.Errors.Shop.ShopNotFound;
            }
        }

        List<Category> articleCategories = categories.Where(x => categoryIds.Contains(x.Id)).ToList();
       

        List<MODEL.Entities.Shop> shops = new();

        foreach (var shopId in shopIds)
        {
            shops.Add(await _shopRepository.GetShop(shopId));
        }

        MODEL.Entities.Article? article = await _articleRepository.GetArticleByName(commerceId, name);

        if (article is not null) return Errors.Errors.Article.ArticleAddedAlready;

        MODEL.Entities.Article newArticle = new()
        {
            Name = name,
            Cur = currency,
            Description = description,
            OldPrice = oldPrice,
            NewPrice = newPrice,
            ValidDate = (DateTime)BaseHelper.ConvertStringToDateTime(validDate),
            Sale = BaseHelper.CalculateSale(oldPrice, newPrice),
            Shops = shops,
            Categories = articleCategories
        };

        _articleRepository.Save<MODEL.Entities.Article>(newArticle);
        return newArticle;
        //TODO: sending the mail to user on article creation
    }

    public async Task<ErrorOr<bool>> DeleteArticle(Guid id)
    {
        MODEL.Entities.Article? article = await _articleRepository.GetById<Article>(id);

        if (article is null)
            return Errors.Errors.Article.ArticleNotFound;

        article.IsDeleted = true;

        _articleRepository.UpdateEntity<MODEL.Entities.Article>(article!);
        return true;
    }

    public async Task<ErrorOr<MODEL.Entities.Article?>> UpdateArticle(Guid articleId, string? name, string? description, decimal? newPrice, string? validDate)
    {
        MODEL.Entities.Article? article = await _articleRepository.GetById<MODEL.Entities.Article>(articleId);

        if (article is null)
            return Errors.Errors.Article.ArticleNotFound;

        if (name is not null) article.Name = name;

        if (description is not null) article.Description = description;

        if (newPrice is not null)
        {
            article.NewPrice = (decimal)newPrice;
            article.Sale = BaseHelper.CalculateSale(article.OldPrice, newPrice.Value);
        }

        if (validDate is not null)
        {
            DateTime date = (DateTime)BaseHelper.ConvertStringToDateTime(validDate);
            article.ValidDate = date;
        }

        _articleRepository.UpdateEntity<MODEL.Entities.Article>(article!);

        return article;
    }
}
