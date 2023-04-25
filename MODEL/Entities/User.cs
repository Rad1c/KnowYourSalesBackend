namespace MODEL.Entities
{
    public partial class User : BaseEntity
    {
        public User()
        {
            FavoriteArticles = new HashSet<FavoriteArticle>();
            FavoriteCommerces = new HashSet<FavoriteCommerce>();
            Impressions = new HashSet<Impression>();
        }

        public Guid AccId { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public char Sex { get; set; }
        public DateOnly Birthdate { get; set; }

        public virtual Account Acc { get; set; } = null!;
        public virtual ICollection<FavoriteArticle> FavoriteArticles { get; set; }
        public virtual ICollection<FavoriteCommerce> FavoriteCommerces { get; set; }
        public virtual ICollection<Impression> Impressions { get; set; }
    }
}
