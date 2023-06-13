namespace API.Models.Common;

public record GeoPointModel
{
    public string Longitude { get; init; }
    public string Latitude { get; init; }
    public string Name { get; init; } = null!;
    public string Address { get; init; } = null!;

}
