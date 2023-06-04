namespace MODEL.QueryModels.User;

public record UserImpressionQueryModel
{
    public string Name { get; init; } = null!;
    public string Impression { get; init; } = null!;
}
