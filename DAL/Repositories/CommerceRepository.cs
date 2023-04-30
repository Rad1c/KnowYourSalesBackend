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

    public Task<Commerce?> GetCommerceByEmail(string email)
    {
        return _context.Commerces
            .Where(u => u.IsDeleted == false)
            .Include(u => u.Acc)
                .ThenInclude(a => a.Role)
            .FirstOrDefaultAsync(u => u.Acc.Email == email && !u.Acc.IsDeleted);
    }

    public async Task<Commerce?> GetCommerceById(Guid id)
    {
        return await _context.Commerces
            .Where(u => u.Id == id && !u.IsDeleted)
            .Include(u => u.Acc)
            .FirstOrDefaultAsync();
    }

    public async Task<CommerceQueryModel?> GetCommerceQuery(Guid id)
    {
        string query = "SELECT com.id, com.name, com.logo, c.name as \"city\", acc.email from commerce com JOIN city c ON c.id = com.cit_id JOIN account acc ON acc.id = com.acc_id WHERE acc.is_deleted = false";
        using var connection = _queryContext.CreateConnection();

        var commerce = await connection.QuerySingleOrDefaultAsync<CommerceQueryModel>(query,
                new { id });

        return commerce;
    }
}
