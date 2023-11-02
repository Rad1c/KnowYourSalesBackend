namespace MODEL.QueryModels.ReferenteData;

public record ArticleQueryModel
{
    public Guid Id { get; init; }
    public Guid CommerceId { get; init; }
    public Guid CategoryId { get; init; }
    public Guid CityId { get; init; }
    public string Name { get; init; } = null!;
    public decimal OldPrice { get; init; }
    public decimal NewPrice { get; init; }
    public decimal Sale { get; init; }
    public string Created { get; init; } = null!;
    public string ValidDate { get; init; } = null!;
    public string Picture { get; init; } = null!;
    public string Logo { get; init; } = null!;
}