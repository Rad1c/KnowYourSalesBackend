namespace MODEL.Entities
{
    public partial class Category : BaseEntity
    {
        public Category()
        {
            Arts = new HashSet<Article>();
        }

        public string Name { get; set; } = null!;
        public short DisplaySeq { get; set; }

        public virtual ICollection<Article> Arts { get; set; }
    }
}
