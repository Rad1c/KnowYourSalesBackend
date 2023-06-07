namespace MODEL.Entities
{
    public partial class Shop : BaseEntity
    {
        public Shop()
        {
            Articles = new HashSet<Article>();
        }

        public Guid CommerceId { get; set; }
        public Guid CityId { get; set; }
        public Guid GeoId { get; set; }
        public string Name { get; set; } = null!;
        public virtual City City { get; set; } = null!;
        public virtual Commerce Commerce { get; set; } = null!;
        public virtual GeoPoint GeoPoint { get; set; } = null!;

        public virtual ICollection<Article> Articles { get; set; }
    }
}
