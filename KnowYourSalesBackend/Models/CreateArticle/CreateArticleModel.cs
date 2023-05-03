namespace API.Models.CreateArticle;

public record CreateArticleModel
{
    public Guid CommerceId { get; set; }
    public List<Guid> CategoryIds { get; init; } = null!;
    public List<Guid> ShopIds { get; init; } = null!;
    public string Name { get; init; } = null!;
    public string Description { get; set; } = null!;
    public decimal OldPrice { get; set; }
    public decimal NewPrice { get; set; }
    public string ValidDate { get; set; } = null!;
}
