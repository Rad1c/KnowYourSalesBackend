namespace MODEL.QueryModels.Common;

public record GeoQueryModel
{
    public decimal Longitude { get; init; }
    public decimal Latitude { get; init; }
    public string Address { get; init; } = string.Empty;
}
