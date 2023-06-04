namespace API.Models.GetCitiesByCountryCode;

public record GetCitiesByCountryCodeQueryModel
{
    public string Code { get; init; } = null!;
}
