using API.Models.Common;
using FluentValidation;

namespace API.Models.CreateShop;

public class CreateShopModelValidator : AbstractValidator<CreateShopModel>
{
    public CreateShopModelValidator()
    {
        RuleFor(x => x.Name).NotEmpty();

        RuleFor(x => x.CommerceId).NotNull().NotEmpty();

        RuleFor(x => x.CityId).NotNull().NotEmpty();

        RuleFor(x => x.GeoPoint)
            .SetValidator(new GeoPointValidator());
    }
}

public class GeoPointValidator : AbstractValidator<GeoPointModel>
{
    public GeoPointValidator()
    {
        RuleFor(x => x.Latitude).NotNull();

        RuleFor(x => x.Longitude).NotNull();
    }
}

