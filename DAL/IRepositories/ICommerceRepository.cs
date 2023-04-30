using MODEL.Entities;

namespace DAL.IRepositories
{
    public interface ICommerceRepository : IRepository<Commerce>
    {
        public Task<Commerce?> GetCommerceByEmail(string email);
        public Task<Role?> GetRoleByCode(string code, bool? isDeleted = false);
    }
}
