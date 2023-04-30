using MODEL.Entities;

namespace DAL.IRepositories;
public interface IShopRepository : IRepository<Commerce>
{
    public Task<Commerce?> GetCommerceById(Guid id);
}

