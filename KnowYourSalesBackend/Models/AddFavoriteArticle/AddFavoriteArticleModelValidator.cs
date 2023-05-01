using FluentValidation;

namespace API.Models.AddFavoriteArticle;

public class AddFavoriteArticleModelValidator : AbstractValidator<AddFavoriteArticleModel>
{
    public AddFavoriteArticleModelValidator()
    {
        RuleFor(x => x.ArticleId).NotNull().NotEmpty();
    }
}
