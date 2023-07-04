namespace MODEL.Entities
{
    public partial class Picture : BaseEntity
    {
        public Guid ArticleId { get; set; }
        public string PicUrl { get; set; } = null!;
        public bool IsThumbnail { get; set; }

        public virtual Article Article { get; set; } = null!;
    }
}
