namespace API.Models;

public record GetCitiesByCountryCodeQueryModel
{
    public string Code { get; init; } = null!;
}
