namespace MODEL.Entities
{
    public partial class Country : BaseEntity
    {
        public Country()
        {
            Cities = new HashSet<City>();
        }

        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;

        public virtual ICollection<City> Cities { get; set; }
    }
}
