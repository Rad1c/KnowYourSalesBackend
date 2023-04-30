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
}

