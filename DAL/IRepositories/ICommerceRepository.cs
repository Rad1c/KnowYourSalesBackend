using MODEL.Entities;
using MODEL.QueryModels.Commerce;

namespace DAL.IRepositories
{
    public interface ICommerceRepository : IRepository<Commerce>
    {
        public Task<Commerce?> GetCommerceByEmail(string email);
        public Task<Commerce?> GetCommerceById(Guid id);
        public Task<CommerceQueryModel?> GetCommerceQuery(Guid id);
    }
}
