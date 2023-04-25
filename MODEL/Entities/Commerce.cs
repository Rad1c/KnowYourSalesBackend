namespace MODEL.Entities
{
    public partial class Commerce : BaseEntity
    {
        public Commerce()
        {
            FavoriteCommerces = new HashSet<FavoriteCommerce>();
            Shops = new HashSet<Shop>();
        }

        public Guid CitId { get; set; }
        public Guid AccId { get; set; }
        public string Name { get; set; } = null!;
        public string Logo { get; set; } = null!;

        public virtual Account Acc { get; set; } = null!;
        public virtual City Cit { get; set; } = null!;
        public virtual ICollection<FavoriteCommerce> FavoriteCommerces { get; set; }
        public virtual ICollection<Shop> Shops { get; set; }
    }
}
