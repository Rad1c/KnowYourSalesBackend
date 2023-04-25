namespace MODEL.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime Modified { get; set; }
        public DateTime Created { get; set; }
    }
}
