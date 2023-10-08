namespace API.Models;

public record VerifyAccountModel
{
    public string Code { get; init; } = null!;
}
