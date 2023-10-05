using FluentValidation;

namespace API.Models.Validators;

public class LoginModelValidator : AbstractValidator<LoginModel>
{
    public LoginModelValidator()
    {
        RuleFor(x => x.Email)
            .NotNull().NotEmpty().WithMessage("email is required.")
            .EmailAddress().WithMessage("must be valid email address.");

        RuleFor(x => x.Password)
            .MinimumLength(8)
            .NotNull().NotEmpty().WithMessage("password is required.")
            .Matches("^[^£# “”]*$").WithMessage("'{PropertyName}' must not contain the following characters £ # “” or spaces.");
    }
}

