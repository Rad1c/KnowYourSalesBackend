namespace MODEL.Entities
{
    public partial class GeoPoint : BaseEntity
    {
        public GeoPoint()
        {
            Shops = new HashSet<Shop>();
        }

        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public string? Address { get; set; }

        public virtual ICollection<Shop> Shops { get; set; }
    }
}
