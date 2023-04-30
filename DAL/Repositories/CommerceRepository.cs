using DAL.IRepositories;
using Microsoft.EntityFrameworkCore;
using MODEL.Entities;

namespace DAL.Repositories
{
    public class CommerceRepository : Repository, ICommerceRepository
    {
        private readonly Context _context;
        public CommerceRepository(Context context) : base(context)
        {
            _context = context;
        }

        public Task<Commerce?> GetCommerceByEmail(string email)
        {
            return _context.Commerces
                .Where(u => u.IsDeleted == false)
                .Include(u => u.Acc)
                    .ThenInclude(a => a.Role)
                .FirstOrDefaultAsync(u => u.Acc.Email == email && !u.Acc.IsDeleted);
        }

        public async Task<Role?> GetRoleByCode(string code, bool? isDeleted = false)
        {
            return await _context.Roles
                .Where(r => r.Code == code && r.IsDeleted == isDeleted)
                .FirstOrDefaultAsync();
        }
    }
}
