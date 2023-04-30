namespace API.Models.UpdateUser;

public record UpdateUserModel
{
    public Guid Id { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? Sex { get; init; }
    public string? DateOfBirth { get; init; }
}

