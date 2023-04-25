namespace MODEL.Entities
{
    public partial class Impression : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid UseId { get; set; }
        public DateOnly Created { get; set; }
        public DateOnly? Modified { get; set; }
        public bool IsDeleted { get; set; }
        public string Content { get; set; } = null!;

        public virtual User Use { get; set; } = null!;
    }
}
