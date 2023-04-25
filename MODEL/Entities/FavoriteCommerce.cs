namespace MODEL.Entities
{
    public partial class FavoriteCommerce : BaseEntity
    {
        public Guid? ComId { get; set; }
        public Guid? UseId { get; set; }

        public virtual Commerce? Com { get; set; }
        public virtual User? Use { get; set; }
    }
}
