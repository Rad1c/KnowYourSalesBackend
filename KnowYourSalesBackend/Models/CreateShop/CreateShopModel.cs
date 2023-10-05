namespace API.Models;

public record CreateShopModel
{
    public string Name { get; init; } = null!;
    public Guid CityId { get; init; }
    public GeoPointModel GeoPoint { get; init; } = null!;
}
