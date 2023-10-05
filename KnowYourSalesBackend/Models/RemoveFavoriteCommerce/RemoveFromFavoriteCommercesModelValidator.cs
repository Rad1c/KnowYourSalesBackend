using FluentValidation;

namespace API.Models.Validators;

public class RemoveFromFavoriteCommercesModelValidator : AbstractValidator<RemoveFromFavoriteCommercesModel>
{
    public RemoveFromFavoriteCommercesModelValidator()
    {
        RuleFor(x => x.CommerceId).NotNull().NotEmpty();
    }
}
