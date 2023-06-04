using FluentValidation;

namespace API.Models.RegisterCommerce
{
    public class RegisterCommerceModelValidator : AbstractValidator<RegisterCommerceModel>
    {
        public RegisterCommerceModelValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage("name is required.");

            RuleFor(x => x.CityId).NotNull().NotEmpty();

            RuleFor(x => x.Email)
            .NotNull().NotEmpty().WithMessage("email is required.")
            .EmailAddress().WithMessage("must be valid email address.");

            RuleFor(x => x.Password)
            .MinimumLength(8)
            .NotNull().NotEmpty().WithMessage("password is required.")
            .Matches("^[^£# “”]*$").WithMessage("'{PropertyName}' must not contain the following characters £ # “” or spaces.");

            RuleFor(x => x.ConfirmPassword)
                .NotNull().NotEmpty().WithMessage("confirm password is required.")
                .Equal(x => x.Password).WithMessage("passwords do not match.");
        }

    }
}
