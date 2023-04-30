using FluentValidation;

namespace API.Models.UpdateCommerce
{
    public class UpdateCommerceModelValidator : AbstractValidator<UpdateCommerceModel>
    {
        public UpdateCommerceModelValidator()
        {
            RuleFor(x => x.CommerceId).NotNull().NotEmpty();
        }
    }
}
