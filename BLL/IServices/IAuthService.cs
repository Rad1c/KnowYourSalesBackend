using BLL.Enums;
using ErrorOr;
using MODEL.Entities;

namespace BLL.IServices;

public interface IAuthService
{
    bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
    void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
    string CreateToken(Guid userId, Guid accountId, TokenTypeEnum tokenType, RoleEnum role);
    Dictionary<string, string> ValidateToken(string token);
    public Task<ErrorOr<Account?>> GetAccountByEmail(string email);

}

