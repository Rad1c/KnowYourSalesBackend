namespace MODEL.Entities
{
    public partial class Impression : BaseEntity
    {
        public Guid UseId { get; set; }
        public string Content { get; set; } = null!;

        public virtual User Use { get; set; } = null!;
    }
}
