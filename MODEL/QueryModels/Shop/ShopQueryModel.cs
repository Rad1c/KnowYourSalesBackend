namespace MODEL.QueryModels.Shop;

public record ShopQueryModel
{
    public Guid Id { get; set; }
    public Guid CommerceId { get; set; }
    public Guid CityId { get; set; }   
    public string CityName { get; set; } = null!;
    public string Name { get; set; } = null!;
    public decimal Longitude { get; set; }
    public decimal Latitude { get; set; }
    public string Address { get; set; } = null!;
}
