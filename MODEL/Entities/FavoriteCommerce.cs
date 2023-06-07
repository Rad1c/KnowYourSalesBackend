namespace MODEL.Entities
{
    public partial class FavoriteCommerce : BaseEntity
    {
        public Guid? CommerceId { get; set; }
        public Guid? UserId { get; set; }
        public virtual Commerce? Commerce { get; set; }
        public virtual User? User { get; set; }
    }
}
