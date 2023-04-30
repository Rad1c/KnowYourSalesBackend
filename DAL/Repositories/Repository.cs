using Microsoft.EntityFrameworkCore;
using MODEL.Entities;

namespace DAL.Repositories;

public class Repository
{
    private readonly Context _context;

    public Repository(Context context)
    {
        _context = context;
    }

    public async Task<T?> GetById<T>(Guid id, bool validOnly = true) where T : BaseEntity
    {
        if (validOnly)
        {
            return await _context.Set<T>().Where(x => x.Id == id && x.IsDeleted == false).FirstOrDefaultAsync();
        }

        return await _context.Set<T>().Where(x => x.Id == id).FirstOrDefaultAsync();
    }

    public void Save<T>(T entity) where T : BaseEntity
    {
        if (entity == null) return;

        _context.Add(entity);
        _context.SaveChanges();
    }

    public async Task<Role?> GetRoleByCode(string code, bool? isDeleted = false)
    {
        return await _context.Roles
            .Where(r => r.Code == code && r.IsDeleted == isDeleted)
            .FirstOrDefaultAsync();
    }

    public void UpdateEntity<T>(T entity) where T : BaseEntity
    {
        _context.Update(entity);
        _context.SaveChanges();
    }
}
