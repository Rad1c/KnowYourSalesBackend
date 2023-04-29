using FluentValidation;

namespace API.Models.Login
{
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
                .Matches("[A-Z]").WithMessage("'{PropertyName}' must contain one or more capital letters.")
                .Matches("[a-z]").WithMessage("'{PropertyName}' must contain one or more lowercase letters.")
                .Matches(@"\d").WithMessage("'{PropertyName}' must contain one or more digits.")
                .Matches(@"[][""!@$%^&*(){}:;<>,.?/+_=|'~\\-]").WithMessage("'{ PropertyName}' must contain one or more special characters.")
                .Matches("^[^£# “”]*$").WithMessage("'{PropertyName}' must not contain the following characters £ # “” or spaces.");

            RuleFor(x => x.IsUser).NotNull();
        }
    }
}
