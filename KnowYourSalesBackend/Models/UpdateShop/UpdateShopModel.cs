using API.Models.Common;

namespace API.Models.UpdateShop;

public record UpdateShopModel
{
    public Guid CommerceId { get; init; }
    public Guid Id { get; init; }
    public string? Name { get; init; } = null!;
    public Guid? CityId { get; init; }
    public GeoPointModel? GeoPoint { get; init; } = null!;
}
