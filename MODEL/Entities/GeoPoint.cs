namespace MODEL.Entities
{
    public partial class GeoPoint : BaseEntity
    {
        public GeoPoint()
        {
            Shops = new HashSet<Shop>();
        }

        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }

        public virtual ICollection<Shop> Shops { get; set; }
    }
}
