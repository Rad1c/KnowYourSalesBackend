using MODEL.Entities;

namespace DAL.IRepositories;
public interface IShopRepository : IRepository<Commerce>
{
    public Task<Commerce?> GetCommerceById(Guid id);
    public Task<Commerce?> GetCommerceWithShops(Guid id);
    public Task<Shop?> GetShop(Guid id);
    public Task<List<Category>> GetCategories();
}

