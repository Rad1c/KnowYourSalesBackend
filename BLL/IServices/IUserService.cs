using ErrorOr;
using MODEL.Entities;

namespace BLL.IServices
{
    public interface IUserService
    {
        public Task<ErrorOr<User?>> RegisterUser(string firstName, string lastName, string dateOfBirth, byte[] passwordHash, byte[] salt, string sex, string email);
    }
}
