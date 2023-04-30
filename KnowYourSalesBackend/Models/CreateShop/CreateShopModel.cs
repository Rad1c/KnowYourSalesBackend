namespace API.Models.CreateShop;

public record CreateShopModel
{
    public string Name { get; init; } = null!;
    public Guid CommerceId { get; init; }
    public Guid CityId { get; init; }
    public GeoPoint GeoPoint { get; init; } = null!;
}

public record GeoPoint
{
    public decimal Longitude { get; init; }
    public decimal Latitude { get; init; }
    public string Name { get; init; } = null!;
    public string Address { get; init; } = null!;

}
