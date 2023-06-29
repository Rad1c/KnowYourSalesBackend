using ErrorOr;

namespace BLL.Errors;

public partial class Errors
{
    public static class ArticleEr
    {
        public static Error ArticleNotFound => Error.NotFound(code: "Article.ArticleNotFound", "article not found.");
        public static Error ArticleAddedAlready => Error.Conflict(code: "Article.ArticleAddedAlready", description: "article is already added");
        public static Error CurrencyNotFound => Error.Failure(code: "Article.CurrencyNotExist", description: "currency not exist");
    }
}
