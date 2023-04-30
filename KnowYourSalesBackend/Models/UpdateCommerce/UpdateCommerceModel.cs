namespace API.Models.UpdateCommerce
{
    public record UpdateCommerceModel
    {
        public Guid CommerceId { get; init; } //TODO: from teken
        public string? Name { get; init; } = null!;
        public Guid? CityId { get; init; }
        public string? Logo { get; init; } = null!;
    }
}
