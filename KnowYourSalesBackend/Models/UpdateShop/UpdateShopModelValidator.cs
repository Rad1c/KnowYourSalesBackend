using FluentValidation;

namespace API.Models.Validators;

public class UpdateShopModelValidator : AbstractValidator<UpdateShopModel>
{
    public UpdateShopModelValidator()
    {
        RuleFor(x => x.Id).NotNull().NotEmpty();
    }
}
