using FluentValidation;

namespace API.Models.RemoveFavoriteCommerce;

public class RemoveFromFavoriteCommercesModelValidator : AbstractValidator<RemoveFromFavoriteCommercesModel>
{
    public RemoveFromFavoriteCommercesModelValidator()
    {
        RuleFor(x => x.CommerceId).NotNull().NotEmpty();
    }
}
