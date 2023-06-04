using FluentValidation;

namespace API.Models.GetCitiesByCountryCode
{
    public class GetCitiesByCountryCodeQueryModelValidator : AbstractValidator<GetCitiesByCountryCodeQueryModel>
    {
        public GetCitiesByCountryCodeQueryModelValidator()
        {

            RuleFor(x => x.Code).NotNull().NotEmpty().Must(x => x.Length == 2);
        }
    }
}
