namespace API.Models;

public record UpdateArticleModel
{
    public Guid Id { get; set; }
    public string Name { get; init; } = null!;
    public string Description { get; set; } = null!;
    public decimal OldPrice { get; set; }
    public decimal NewPrice { get; set; }
    public string ValidDate { get; set; } = null!;
}
