namespace MODEL.QueryModels.ReferenceData;

public record CityQueryModel
{
    public Guid CityId { get; set; }
    public string CityName { get; set; }
}
