using MODEL.Entities;

namespace DAL.IRepositories;

public interface IAuthRepository : IRepository<Account>
{
    public Task<Account?> GetAccountByEmail(string email);
    public Task<Account?> GetAccountByEmailVerificationCode(string code);
}
