﻿using DAL.IRepositories;
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
            .Include(u => u.Account)
                .ThenInclude(a => a.Role)
            .FirstOrDefaultAsync(u => u.Account.Email == email && !u.Account.IsDeleted);
    }

    public async Task<User?> GetUserById(Guid id)
    {
        return await _context.Users
            .Where(u => u.Id == id && !u.IsDeleted)
            .Include(u => u.Account)
            .FirstOrDefaultAsync();
    }

    public async Task<User?> GetUserByAccountId(Guid accId)
    {
        return await _context.Users.Where(c => !c.IsDeleted)
            .Include(c => c.Account)
            .Where(c => !c.Account.IsDeleted && c.AccountId == accId)
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

    public async Task<List<FavoriteCommerceQueryModel>> GetFavoriteCommercesQuery(Guid userId)
    {
        string query = @"SELECT c.id, c.name, c.logo FROM favorite_commerce f 
                JOIN commerce c ON c.id = f.com_id JOIN ""user"" u on u.id = f.use_id 
                WHERE f.is_deleted = false AND c.is_deleted = false AND u.id = @userId";
        using var connection = _queryContext.CreateConnection();

        var favCommerces = await connection.QueryAsync<FavoriteCommerceQueryModel>(query, new { userId });

        return (List<FavoriteCommerceQueryModel>)favCommerces;
    }

    public async Task<List<UserImpressionQueryModel>> GetImpressions(int? limit = 4)
    {
        string query = $"SELECT u.name, i.content as impression from impression i JOIN \"user\" u ON u.id = i.use_id LIMIT {limit}";

        using var connection = _queryContext.CreateConnection();

        var impressions = await connection.QueryAsync<UserImpressionQueryModel>(query);

        return (List<UserImpressionQueryModel>)impressions;
    }

    public async Task<User?> GetUserWithFavoriteCommerces(Guid id)
    {
        return await _context.Users
            .Where(u => u.Id == id && !u.IsDeleted)
            .Include(u => u.FavoriteCommerces.Where(fc => !fc.IsDeleted))
            .FirstOrDefaultAsync();
    }

    public async Task<User?> GetUserWithImpressions(Guid id)
    {
        return await _context.Users
            .Where(u => u.Id == id && !u.IsDeleted)
            .Include(u => u.Impressions)
            .FirstOrDefaultAsync();
    }

    public async Task<User?> GetUserWithFavoriteArticles(Guid id)
    {
        return await _context.Users
            .Where(u => !u.IsDeleted && u.Id == id)
            .Include(u => u.FavoriteArticles.Where(fa => !fa.IsDeleted))
            .FirstOrDefaultAsync();
    }
}
