using BLL.IServices;
using DAL.IRepositories;
using MODEL.Entities;

namespace BLL.Services;

public class ShopService : IShopService
{
    private readonly IShopRepository _commerceRepository;

    public ShopService(IShopRepository commerceRepository)
    {
        _commerceRepository = commerceRepository;
    }

    public Task<Commerce?> GetCommerceById(Guid id) => _commerceRepository.GetCommerceById(id);
}

