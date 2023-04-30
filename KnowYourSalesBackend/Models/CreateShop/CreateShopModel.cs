using API.Models.Common;

namespace API.Models.CreateShop;

public record CreateShopModel
{
    public string Name { get; init; } = null!;
    public Guid CommerceId { get; init; }
    public Guid CityId { get; init; }
    public GeoPointModel GeoPoint { get; init; } = null!;
}
