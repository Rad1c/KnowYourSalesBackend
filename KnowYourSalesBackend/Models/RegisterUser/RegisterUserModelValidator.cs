using BLL.Helper;
using FluentValidation;

namespace API.Models.Validators;

public class RegisterUserModelValidator : AbstractValidator<RegisterUserModel>
{
    public RegisterUserModelValidator()
    {
        RuleFor(x => x.FirstName)
            .NotNull()
            .NotEmpty().WithMessage("first name is required.")
            .MinimumLength(3)
            .MaximumLength(25);

        RuleFor(x => x.LastName)
            .NotNull().NotEmpty().WithMessage("Last name is required.")
            .MinimumLength(3)
            .MaximumLength(25);

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

        RuleFor(x => x.DateOfBirth).NotEmpty()
            .Matches(@"^\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}$").WithMessage("Timestamp must be in ISO 8601 format.")
            .Must(x => !BaseHelper.DateTimeGreaterThanNow(BaseHelper.ConvertStringToDateTime(x)))
            .WithMessage("Bad MessageTimestamp.");

        RuleFor(x => x.Sex)
            .NotEmpty()
            .Must(x => x == "M" || x == "F");
    }
}
