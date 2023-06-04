using MODEL.Entities;

namespace DAL.IRepositories;

public interface IAuthRepository
{
    public Task<Account?> GetAccountByEmail(string email);
}
