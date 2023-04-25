namespace MODEL.Entities
{
    public partial class Currency : BaseEntity
    {
        public Currency()
        {
            Articles = new HashSet<Article>();
        }

        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public string Label { get; set; } = null!;

        public virtual ICollection<Article> Articles { get; set; }
    }
}
