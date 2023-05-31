using BLL.Helper;
using FluentValidation;

namespace API.Models.CreateArticle;

public class CreateArticleModelValidator : AbstractValidator<CreateArticleModel>
{
    public CreateArticleModelValidator()
    {
        RuleFor(x => x.CommerceId).NotNull().NotEmpty();

        RuleForEach(x => x.ShopIds).NotNull().NotEmpty();

        RuleForEach(x => x.CategoryIds).NotNull().NotEmpty();

        RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage("name is required.");

        RuleFor(x => x.Description).NotNull().NotEmpty().WithMessage("description is required.");

        RuleFor(x => x.OldPrice).NotNull().NotEmpty().WithMessage("old price is required.").GreaterThan(0)
            .WithMessage("price must be greater than 0.").PrecisionScale(7, 2, true).WithMessage("price must be within the scale (0,00 - 9999,00)");

        RuleFor(x => x.NewPrice).NotNull().NotEmpty().WithMessage("new price is required.").GreaterThan(0)
            .WithMessage("price must be greater than 0.").PrecisionScale(7, 2, true).WithMessage("price must be within the scale (0,00 - 9999,00)")
            .LessThan(x => x.OldPrice).WithMessage("new price must be lower than the old price");

        RuleFor(x => x.ValidDate).NotEmpty()
            .Matches(@"^\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}$").WithMessage("Timestamp must be in ISO 8601 format.")
            .Must(x => BaseHelper.DateTimeGreaterThanNow(BaseHelper.ConvertStringToDateTime(x)))
            .WithMessage("Bad MessageTimestamp.");
    }
}
