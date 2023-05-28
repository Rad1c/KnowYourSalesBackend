namespace MODEL.QueryModels.ReferenteData;

public class CountryQueryModel
{
    public Guid CountryId { get; set; }
    public string CountryName { get; set; } = null!;
    public List<CityQueryModel> Cities { get; set; } = null!;
}
