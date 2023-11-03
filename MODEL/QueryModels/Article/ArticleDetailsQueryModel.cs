using MODEL.QueryModels.Common;

namespace MODEL.QueryModels.Article;

public record ArticleDetailsQueryModel
{
    public Guid ArticleId { get; init; }
    public Guid CommerceId { get; init; }
    public Guid CategoryId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public decimal OldPrice { get; init; }
    public decimal NewPrice { get; init; }
    public decimal Sale { get; init; }
    public DateTime ValidDate { get; init; }
    public string CommerceLogo { get; init; } = string.Empty;
    public IEnumerable<string> Images { get; set; } = Enumerable.Empty<string>();
    public IEnumerable<GeoQueryModel> GeoLocations { get; set; } = Enumerable.Empty<GeoQueryModel>();
}
