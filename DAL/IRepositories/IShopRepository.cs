using MODEL.Entities;
using MODEL.QueryModels.Shop;

namespace DAL.IRepositories;
public interface IShopRepository : IRepository<Commerce>
{
    public Task<Commerce?> GetCommerceById(Guid id);
    public Task<Commerce?> GetCommerceWithShops(Guid id);
    public Task<Shop?> GetShop(Guid id);
    public Task<List<Category>> GetCategories();
    public Task<Currency?> GetCurrencyByName(string name);
    public Task<ShopQueryModel?> GetShopQuery(Guid id);
    public Task<List<ShopQueryModel>> GetShopsQuery(Guid id);
}

