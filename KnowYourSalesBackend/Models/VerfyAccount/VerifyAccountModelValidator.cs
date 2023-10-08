using FluentValidation;

namespace API.Models.Validators;

public class VerifyAccountModelValidator : AbstractValidator<VerifyAccountModel>
{
    public VerifyAccountModelValidator()
    {
        RuleFor(x => x.Code).NotNull().NotEmpty().MinimumLength(5).MaximumLength(250);
    }
}
