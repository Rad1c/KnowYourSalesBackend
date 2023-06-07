namespace MODEL.Entities
{
    public partial class Impression : BaseEntity
    {
        public Guid UserId { get; set; }
        public string Content { get; set; } = null!;

        public virtual User User { get; set; } = null!;
    }
}
