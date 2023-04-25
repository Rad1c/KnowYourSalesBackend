namespace MODEL.Entities
{
    public partial class Geopint : BaseEntity
    {
        public Geopint()
        {
            Shops = new HashSet<Shop>();
        }

        public string Longitude { get; set; } = null!;
        public string Latitude { get; set; } = null!;

        public virtual ICollection<Shop> Shops { get; set; }
    }
}
