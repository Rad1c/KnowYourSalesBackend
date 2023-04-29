using BLL.Enums;

namespace BLL.IServices
{
    public interface IAuthService
    {
        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        string CreateToken(Guid userId, TokenTypeEnum tokenType, RoleEnum role);
        Dictionary<string, string> ValidateToken(string token);

    }
}
