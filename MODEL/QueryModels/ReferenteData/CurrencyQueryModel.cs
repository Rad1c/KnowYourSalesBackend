namespace MODEL.QueryModels.ReferenteData;

public record CurrencyQueryModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Code { get; set; } = null!;
}
