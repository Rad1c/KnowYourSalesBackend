using FluentValidation;

namespace API.Models.Validators;

public class UserImpressionModelValidator : AbstractValidator<UserImpressionModel>
{
    public UserImpressionModelValidator()
    {
        RuleFor(x => x.Impression).NotEmpty();
    }
}
