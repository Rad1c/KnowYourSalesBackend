namespace MODEL.Entities
{
    public partial class FavoriteArticle : BaseEntity
    {
        public Guid? ArticleId { get; set; }
        public Guid? UserId { get; set; }
        public virtual Article? Article { get; set; }
        public virtual User? User { get; set; }
    }
}
