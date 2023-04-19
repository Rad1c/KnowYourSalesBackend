namespace MODEL.Entites
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime Updated { get; set; }
        public DateTime Created { get; set; }
    }
}
