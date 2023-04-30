namespace MODEL.QueryModels.Commerce;

public record CommerceQueryModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Logo { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Email { get; set; } = null!;
}
