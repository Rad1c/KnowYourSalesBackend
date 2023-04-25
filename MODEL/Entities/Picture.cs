namespace MODEL.Entities
{
    public partial class Picture : BaseEntity
    {
        public Guid ArtId { get; set; }
        public string Pic { get; set; } = null!;

        public virtual Article Art { get; set; } = null!;
    }
}
