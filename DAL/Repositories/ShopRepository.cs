using DAL.IRepositories;
using Microsoft.EntityFrameworkCore;
using MODEL.Entities;

namespace DAL.Repositories;

public class ShopRepository : Repository, IShopRepository
{
    private readonly Context _context;

    public ShopRepository(Context context) : base(context)
    {
        _context = context;
    }

    public async Task<Commerce?> GetCommerceById(Guid id)
    {
        return await _context.Commerces
            .Where(c => c.Id == id && !c.IsDeleted)
            .Include(c => c.Acc)
            .FirstOrDefaultAsync();
    }

    public async Task<Commerce?> GetCommerceWithShops(Guid id)
    {
        return await _context.Commerces.Where(c => !c.IsDeleted && c.Id == id)
            .Include(x => x.Shops).FirstOrDefaultAsync();
    }

    public async Task<Shop?> GetShop(Guid id)
    {
        return await _context.Shops
            .Where(s => s.Id == id && !s.IsDeleted)
            .Include(s => s.Geo)
            .Include(s => s.Cit)
            .FirstOrDefaultAsync();
    }
}

