namespace MODEL.Entities
{
    public partial class Account : BaseEntity
    {
        public Account()
        {
            Commerces = new HashSet<Commerce>();
            Users = new HashSet<User>();
        }

        public Guid RoleId { get; set; }
        public string Email { get; set; } = null!;
        public byte[] Password { get; set; } = null!;
        public byte[]? Salt { get; set; }

        public virtual Role Role { get; set; } = null!;
        public virtual ICollection<Commerce> Commerces { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
