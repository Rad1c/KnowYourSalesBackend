using FluentValidation;

namespace API.Models.Validators;

public class AddArticleImageModelValidator : AbstractValidator<AddArticleImageModel>
{
    public AddArticleImageModelValidator()
    {
        RuleFor(x => x.ArticleId).NotEmpty().NotNull();

        RuleFor(x => x.Image).NotEmpty().NotNull();

        RuleFor(x => x.isThumbnail).NotNull();
    }
}
