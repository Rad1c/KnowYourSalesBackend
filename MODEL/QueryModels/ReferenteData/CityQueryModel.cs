namespace MODEL.QueryModels.ReferenteData;

public record CityQueryModel
{
    public Guid CityId { get; set; }
    public string CityName { get; set; }
    public decimal Longitude { get; set; }
    public decimal Latitude { get; set; }
}
