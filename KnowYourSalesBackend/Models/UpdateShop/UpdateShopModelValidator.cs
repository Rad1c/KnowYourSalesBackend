using FluentValidation;

namespace API.Models.UpdateShop;

public class UpdateShopModelValidator : AbstractValidator<UpdateShopModel>
{
    public UpdateShopModelValidator()
    {
        RuleFor(x => x.Id).NotNull().NotEmpty();
    }
}
