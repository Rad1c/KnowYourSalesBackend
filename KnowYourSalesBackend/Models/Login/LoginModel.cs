namespace API.Models.Login;

public record LoginModel
{
    public string Email { get; init; } = null!;
    public string Password { get; init; } = null!;
}

