using BLL.Enums;
using ErrorOr;
using MODEL.Entities;
using MODEL.QueryModels.User;

namespace BLL.IServices;

public interface IUserService
{
    public Task<ErrorOr<User?>> RegisterUser(string firstName, string lastName, string dateOfBirth, byte[] passwordHash, byte[] salt, string sex, string email);
    public Task<ErrorOr<User?>> GetUserByEmail(string email);
    public Task<User?> GetUserById(Guid id);
    public Task<ErrorOr<bool>> DeleteUser(Guid userId);
    public Task<ErrorOr<bool>> RemoveCommerceFromFavorites(Guid userId, Guid commerceId);
    public Task<ErrorOr<bool>> UpdateUser(Guid userId, string? firstName, string? lastName, string? dateOfBirth, SexEnum? sex);
    public Task<UserQueryModel> GetUserQuery(Guid userId);
    public Task<ErrorOr<bool>> AddUserImpression(Guid id, string impression);
    public Task<ErrorOr<bool>> AddFavoriteCommerce(Guid userId, Guid commerceId);
    public Task<ErrorOr<bool>> AddFavoriteArticle(Guid userId, Guid articleId);
}
