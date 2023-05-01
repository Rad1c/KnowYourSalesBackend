namespace API.Models.RemoveFavoriteCommerce;

public class RemoveFromFavoriteCommercesModel
{
    public Guid Id { get; init; } //TODO: from token
    public Guid CommerceId { get; init; }
}
