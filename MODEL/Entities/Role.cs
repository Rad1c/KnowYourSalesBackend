namespace MODEL.Entities
{
    public partial class Role : BaseEntity
    {
        public Role()
        {
            Accounts = new HashSet<Account>();
        }

        public string Code { get; set; } = null!;

        public virtual ICollection<Account> Accounts { get; set; }
    }
}
