namespace MODEL.QueryModels.User;

public record FavoriteCommerceQueryModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Logo { get; set; } = null!;
}
