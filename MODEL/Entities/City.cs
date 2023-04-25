namespace MODEL.Entities
{
    public partial class City : BaseEntity
    {
        public City()
        {
            Commerces = new HashSet<Commerce>();
            Shops = new HashSet<Shop>();
        }

        public string CouId { get; set; } = null!;
        public string Name { get; set; } = null!;

        public virtual Country Cou { get; set; } = null!;
        public virtual ICollection<Commerce> Commerces { get; set; }
        public virtual ICollection<Shop> Shops { get; set; }
    }
}
