using DAL.IRepositories;
using Microsoft.EntityFrameworkCore;
using MODEL.Entities;

namespace DAL.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly Context _context;

        public AuthRepository(Context context)
        {
            _context = context;
        }

        public async Task<Account?> GetAccountByEmail(string email)
        {
            return await _context.Accounts
                .Where(a => a.Email.ToLower().Equals(email.ToLower()) && !a.IsDeleted)
                .Include(a => a.Role)
                .FirstOrDefaultAsync();
        }
    }
}
