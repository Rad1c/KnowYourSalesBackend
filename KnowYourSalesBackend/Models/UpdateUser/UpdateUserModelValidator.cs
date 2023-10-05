using BLL.Helper;
using FluentValidation;

namespace API.Models.Validators;

public class UpdateUserModelValidator : AbstractValidator<UpdateUserModel>
{
    public UpdateUserModelValidator()
    {
        RuleFor(x => x.Sex)
            .NotEmpty()
            .Must(x => x == "M" || x == "F");

        RuleFor(x => x.DateOfBirth).NotEmpty()
            .Matches(@"^\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}$").WithMessage("Timestamp must be in ISO 8601 format.")
            .Must(x => !BaseHelper.DateTimeGreaterThanNow(BaseHelper.ConvertStringToDateTime(x)))
                .When(x => x.DateOfBirth != null)
                .WithMessage("bad dateOfBirth");
    }
}
