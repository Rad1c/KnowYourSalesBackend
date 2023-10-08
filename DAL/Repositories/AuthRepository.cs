using DAL.IRepositories;
using Microsoft.EntityFrameworkCore;
using MODEL.Entities;

namespace DAL.Repositories
{
    public class AuthRepository : Repository, IAuthRepository
    {
        private readonly Context _context;

        public AuthRepository(Context context) : base(context)
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

        public async Task<Account?> GetAccountByEmailVerificationCode(string code)
        {
            return await _context.Accounts
                .FirstOrDefaultAsync(a => !a.IsDeleted && !a.IsEmailVerified && a.VerifyEmailCode == code);
        }
    }
}
