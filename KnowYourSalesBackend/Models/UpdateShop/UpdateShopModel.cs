namespace API.Models;

public record UpdateShopModel
{
    public Guid Id { get; init; }
    public string? Name { get; init; } = null!;
    public Guid? CityId { get; init; }
    public GeoPointModel? GeoPoint { get; init; } = null!;
}
