namespace API.Models;

public record ImageModel
{
    public string Image { get; init; } = string.Empty;
    public bool IsThumbnail { get; init; } = false;
}
