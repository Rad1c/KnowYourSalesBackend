namespace MODEL.Entities
{
    public partial class Commerce : BaseEntity
    {
        public Commerce()
        {
            FavoriteCommerces = new HashSet<FavoriteCommerce>();
            Shops = new HashSet<Shop>();
        }

        public Guid CityId { get; set; }
        public Guid AccountId { get; set; }
        public string Name { get; set; } = null!;
        public string? Logo { get; set; } = null!;

        public virtual Account Account { get; set; } = null!;
        public virtual City City { get; set; } = null!;
        public virtual ICollection<FavoriteCommerce> FavoriteCommerces { get; set; }
        public virtual ICollection<Shop> Shops { get; set; }
    }
}
