using FluentValidation;

namespace API.Models.UserImpression;

public class UserImpressionModelValidator : AbstractValidator<UserImpressionModel>
{
    public UserImpressionModelValidator()
    {
        RuleFor(x => x.Impression).NotEmpty();
    }
}
