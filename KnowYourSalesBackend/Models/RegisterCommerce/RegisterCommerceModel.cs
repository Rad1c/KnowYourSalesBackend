namespace API.Models.RegisterCommerce
{
    public record RegisterCommerceModel
    {
        public string Name { get; init; } = null!;
        public Guid CityId { get; init; }
        public string Email { get; init; } = null!;
        public string Password { get; init; } = null!;
        public string ConfirmPassword { get; init; } = null!;
    }
}
