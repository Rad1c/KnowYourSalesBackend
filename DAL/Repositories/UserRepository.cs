using DAL.IRepositories;
using Microsoft.EntityFrameworkCore;
using MODEL.Entities;

namespace DAL.Repositories
{
    public class UserRepository : Repository, IUserRepository
    {
        private readonly Context _context;

        public UserRepository(Context context) : base(context)
        {
            _context = context;
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
    }
}
