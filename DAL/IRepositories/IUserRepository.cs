using MODEL.Entities;
using MODEL.QueryModels.User;

namespace DAL.IRepositories;

public interface IUserRepository : IRepository<User>
{
    public Task<User?> GetUserById(Guid id);
    public Task<User?> GetUserByEmail(string email);
    public Task<UserQueryModel> GetUserQuery(Guid id);
    public Task<User?> GetUserWithImpressions(Guid id);
    public Task<User?> GetUserWithFavoriteCommerces(Guid id);
    public Task<List<FavoriteCommerceQueryModel>> GetFavoriteCommercesQuery(Guid userId);
}
