namespace API.Models;

public record GeoPointModel
{
    public decimal Longitude { get; init; }
    public decimal Latitude { get; init; }
    public string Address { get; init; } = null!;

}
