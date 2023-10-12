namespace MODEL.Entities
{
    public partial class City : BaseEntity
    {
        public City()
        {
            Commerces = new HashSet<Commerce>();
            Shops = new HashSet<Shop>();
        }

        public Guid CountryId { get; set; }
        public string Name { get; set; } = null!;
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }

        public virtual Country Country { get; set; } = null!;
        public virtual ICollection<Commerce> Commerces { get; set; }
        public virtual ICollection<Shop> Shops { get; set; }
    }
}
