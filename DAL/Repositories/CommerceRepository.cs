using DAL.IRepositories;
using Dapper;
using Microsoft.EntityFrameworkCore;
using MODEL;
using MODEL.Entities;
using MODEL.QueryModels.Commerce;

namespace DAL.Repositories;

public class CommerceRepository : Repository, ICommerceRepository
{
    private readonly Context _context;
    private readonly QueryContext _queryContext;
    public CommerceRepository(Context context, QueryContext queryContext) : base(context)
    {
        _context = context;
        _queryContext = queryContext;
    }

    public async Task<Commerce?> GetCommerceByAccountId(Guid accId)
    {
        return await _context.Commerces.Where(c => !c.IsDeleted)
            .Include(c => c.Account)
            .Where(c => !c.Account.IsDeleted && c.AccountId == accId)
            .FirstOrDefaultAsync();
    }

    public Task<Commerce?> GetCommerceByEmail(string email)
    {
        return _context.Commerces
            .Where(u => u.IsDeleted == false)
            .Include(u => u.Account)
                .ThenInclude(a => a.Role)
            .FirstOrDefaultAsync(u => u.Account.Email == email && !u.Account.IsDeleted);
    }

    public async Task<Commerce?> GetCommerceById(Guid id)
    {
        return await _context.Commerces
            .Where(u => u.Id == id && !u.IsDeleted)
            .Include(u => u.Account)
            .FirstOrDefaultAsync();
    }

    public async Task<Commerce?> GetCommerceByName(string name)
        => await _context.Commerces
        .Where(c => c.Name.ToLower().Equals(name.ToLower()) && !c.IsDeleted)
        .FirstOrDefaultAsync();

    public async Task<CommerceQueryModel?> GetCommerceQuery(Guid id)
    {
        string query = "SELECT com.id, com.name, com.logo, c.name as \"city\", acc.email from commerce com JOIN city c ON c.id = com.cit_id JOIN account acc ON acc.id = com.acc_id WHERE acc.is_deleted = false";
        using var connection = _queryContext.CreateConnection();

        var commerce = await connection.QuerySingleOrDefaultAsync<CommerceQueryModel>(query,
                new { id });

        return commerce;
    }
}
