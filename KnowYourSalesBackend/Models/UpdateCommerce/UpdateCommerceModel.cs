namespace API.Models;

public record UpdateCommerceModel
{
    public string? Name { get; init; } = null!;
    public Guid? CityId { get; init; }
    public string? Logo { get; init; } = null!;
}
