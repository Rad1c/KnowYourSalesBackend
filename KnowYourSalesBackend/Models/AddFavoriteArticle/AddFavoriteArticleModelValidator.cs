using FluentValidation;

namespace API.Models.Validators;

public class AddFavoriteArticleModelValidator : AbstractValidator<AddFavoriteArticleModel>
{
    public AddFavoriteArticleModelValidator()
    {
        RuleFor(x => x.ArticleId).NotNull().NotEmpty();
    }
}
