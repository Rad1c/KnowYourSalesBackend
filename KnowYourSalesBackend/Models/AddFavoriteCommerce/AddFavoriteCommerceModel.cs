namespace API.Models.AddFavoriteCommerce;

public record AddFavoriteCommerceModel
{
    public Guid UserId { get; init; } //TODO: from token
    public Guid CommerceId { get; init; }
}
