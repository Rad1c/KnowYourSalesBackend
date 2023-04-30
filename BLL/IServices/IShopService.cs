using MODEL.Entities;

namespace BLL.IServices;

public interface IShopService
{
    public Task<Commerce?> GetCommerceById(Guid id);
}

