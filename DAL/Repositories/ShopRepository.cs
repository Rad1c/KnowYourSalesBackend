using DAL.IRepositories;
using Dapper;
using Microsoft.EntityFrameworkCore;
using MODEL;
using MODEL.Entities;
using MODEL.QueryModels.Shop;
using System.Security.Cryptography;

namespace DAL.Repositories;

public class ShopRepository : Repository, IShopRepository
{
    private readonly Context _context;
    private readonly QueryContext _queryContext;

    public ShopRepository(Context context, QueryContext queryContext) : base(context)
    {
        _context = context;
        _queryContext = queryContext;
    }

    public async Task<Commerce?> GetCommerceById(Guid id)
    {
        return await _context.Commerces
            .Where(c => c.Id == id && !c.IsDeleted)
            .Include(c => c.Account)
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
            .Include(s => s.GeoPoint)
            .Include(s => s.City)
            .FirstOrDefaultAsync();
    }
    public async Task<List<Category>> GetCategories()
    {
        return await _context.Categories
            .ToListAsync();
    }

    public async Task<Currency?> GetCurrencyByName(string name)
        => await _context.Currencies.
        Where(x => x.Name == name)
        .FirstOrDefaultAsync();

    public async Task<ShopQueryModel?> GetShopQuery(Guid id)
    {
        string query = "SELECT * FROM mv_shops WHERE \"Id\" = @id";
        using var connection = _queryContext.CreateConnection();

        var shop = await connection.QueryFirstOrDefaultAsync<ShopQueryModel>(query, new { id });

        return shop;
    }

    public async Task<List<ShopQueryModel>> GetShopsQuery(Guid id)
    {
        string query = "SELECT \"Id\", \"CityName\", \"Address\" FROM mv_shops WHERE \"CommerceId\" = @id";
        using var connection = _queryContext.CreateConnection();

        var shops = await connection.QueryAsync<ShopQueryModel>(query, new { id });
        
        return shops.ToList();
    }
}

