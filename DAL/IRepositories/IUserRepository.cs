using MODEL.Entities;

namespace DAL.IRepositories
{
    public interface IUserRepository
    {
        public Task<User?> GetUser(Guid id);
    }
}
