using FluentValidation;

namespace API.Models.AddFavoriteCommerce;

public class AddFavoriteCommerceModelValidator : AbstractValidator<AddFavoriteCommerceModel>
{
    public AddFavoriteCommerceModelValidator()
    {
        RuleFor(x => x.CommerceId).NotNull().NotEmpty();
    }
}
