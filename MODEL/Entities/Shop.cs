namespace MODEL.Entities
{
    public partial class Shop : BaseEntity
    {
        public Shop()
        {
            Arts = new HashSet<Article>();
        }

        public Guid ComId { get; set; }
        public Guid CitId { get; set; }
        public Guid GeoId { get; set; }
        public string Name { get; set; } = null!;
        public virtual City Cit { get; set; } = null!;
        public virtual Commerce Com { get; set; } = null!;
        public virtual GeoPoint Geo { get; set; } = null!;

        public virtual ICollection<Article> Arts { get; set; }
    }
}
