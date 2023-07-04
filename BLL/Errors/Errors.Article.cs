using ErrorOr;

namespace BLL.Errors;

public partial class Errors
{
    public static class Article
    {
        public static Error ArticleNotFound => Error.NotFound(code: "Article.ArticleNotFound", "article not found.");
        public static Error ArticleAddedAlready => Error.Conflict(code: "Article.ArticleAddedAlready", description: "article is already added");
        public static Error CurrencyNotFound => Error.Failure(code: "Article.CurrencyNotExist", description: "currency not exist");
        public static Error ToMuchImagesPerProduct => Error.Failure(code: "Article.ToMuchImagesPerProduct", description: "to much images per product");
    }
}
