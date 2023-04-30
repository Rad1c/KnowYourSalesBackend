using DAL.IRepositories;
using Dapper;
using Microsoft.EntityFrameworkCore;
using MODEL;
using MODEL.Entities;
using MODEL.QueryModels.User;

namespace DAL.Repositories;

public class UserRepository : Repository, IUserRepository
{
    private readonly Context _context;
    private readonly QueryContext _queryContext;

    public UserRepository(Context context, QueryContext queryContext) : base(context)
    {
        _context = context;
        _queryContext = queryContext;
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        return await _context.Users
            .Where(u => u.IsDeleted == false)
            .Include(u => u.Acc)
                .ThenInclude(a => a.Role)
            .FirstOrDefaultAsync(u => u.Acc.Email == email && !u.Acc.IsDeleted);
    }

    public async Task<User?> GetUserById(Guid id)
    {
        return await _context.Users
            .Where(u => u.Id == id && !u.IsDeleted)
            .Include(u => u.Acc)
            .FirstOrDefaultAsync();
    }

    public async Task<UserQueryModel> GetUserQuery(Guid id)
    {
        string query = "SELECT u.id, u.name as \"FirstName\", u.surname as \"LastName\", a.Email, u.Sex, u.birthdate FROM \"user\" u JOIN account a ON a.id = u.acc_id WHERE u.id = @id and u.is_deleted = false";
        using var connection = _queryContext.CreateConnection();

        var user = await connection.QuerySingleOrDefaultAsync<UserQueryModel>(query,
                new { id });

        return user;
    }

    public async Task<User?> GetUserWithImpressions(Guid id)
    {
        return await _context.Users
            .Where(u => u.Id == id && !u.IsDeleted)
            .Include(u => u.Impressions)
            .FirstOrDefaultAsync();
    }
}
