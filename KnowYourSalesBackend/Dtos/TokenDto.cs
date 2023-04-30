namespace API.Dtos;

public record TokenDto
{
    public string AccessToken { get; init; } = null!;
    public string RefreshToken { get; init; } = null!;
}

