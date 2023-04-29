namespace MODEL.Entities
{
    public partial class FavoriteArticle : BaseEntity
    {
        public Guid? ArtId { get; set; }
        public Guid? UseId { get; set; }
        public virtual Article? Art { get; set; }
        public virtual User? Use { get; set; }
    }
}
