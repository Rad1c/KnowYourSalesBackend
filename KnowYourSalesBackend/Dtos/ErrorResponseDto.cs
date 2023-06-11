namespace API.Dtos;

public record ErrorResponseDto
{
    public string Message { get; init; } = null!;
    public List<string> Errors { get; init; } = new List<string>();
}
