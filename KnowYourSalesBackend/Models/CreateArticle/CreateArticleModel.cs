namespace API.Models;

public record CreateArticleModel
{
    public Guid CommerceId { get; set; }
    public List<Guid> CategoryIds { get; init; } = null!;
    public List<Guid> ShopIds { get; init; } = null!;
    public string CurrencyName { get; init; } = null!;
    public string Name { get; init; } = null!;
    public string Description { get; init; } = null!;
    public decimal OldPrice { get; init; }
    public decimal NewPrice { get; init; }
    public string ValidDate { get; init; } = null!;
    public List<ImageModel> Images { get; init; } = new();
}
