using BLL.Enums;
using BLL.Helper;
using BLL.IServices;
using DAL.IRepositories;
using ErrorOr;
using MODEL.Entities;
using MODEL.QueryModels.User;

namespace BLL.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ErrorOr<bool>> AddFavoriteArticle(Guid userId, Guid articleId)
    {
        MODEL.Entities.User? user = await _userRepository.GetUserWithFavoriteArticles(userId);

        if (user is null) return Errors.Errors.UserEr.UserNotFound;

        if (user.FavoriteArticles != null && user.FavoriteArticles.Any(x => x.ArticleId == articleId))
            return Errors.Errors.UserEr.FavArticleAddedAlready;

        Article? article = await _userRepository.GetById<Article>(articleId);

        if (article is null) return Errors.Errors.UserEr.ArticleNotFound;

        FavoriteArticle favArticle = new()
        {
            Article = article,
            User = user
        };

        user.FavoriteArticles.Add(favArticle);

        _userRepository.UpdateEntity<User>(user);
        return true;
    }

    public async Task<ErrorOr<bool>> AddFavoriteCommerce(Guid userId, Guid commerceId)
    {
        User? user = await _userRepository.GetUserWithFavoriteCommerces(userId);

        if (user is null) return Errors.Errors.UserEr.UserNotFound;

        if (user.FavoriteCommerces != null && user.FavoriteCommerces.Any(x => x.Commerce.Id == commerceId))
            return Errors.Errors.UserEr.FavCommerceAddedAlready;

        Commerce? commerce = await _userRepository.GetById<Commerce>(commerceId);

        if (commerce is null) return Errors.Errors.UserEr.CommerceNotFound;

        FavoriteCommerce favCommerce = new()
        {
            Commerce = commerce,
            User = user
        };

        user.FavoriteCommerces.Add(favCommerce);

        _userRepository.UpdateEntity<User>(user);
        return true;
    }

    public async Task<ErrorOr<bool>> AddUserImpression(Guid id, string impression)
    {
        User? user = await _userRepository.GetUserWithImpressions(id);

        if (user is null) return Errors.Errors.UserEr.UserNotFound;

        Impression newImpression = new()
        {
            Content = impression,
            User = user
        };

        user.Impressions.Add(newImpression);

        _userRepository.UpdateEntity<User>(user);
        return true;
    }

    public async Task<ErrorOr<bool>> DeleteUser(Guid userId)
    {
        User? user = await _userRepository.GetUserById(userId);

        if (user is null)
            return Errors.Errors.UserEr.UserNotFound;

        user.Account.IsDeleted = true;
        user.IsDeleted = true;

        _userRepository.UpdateEntity<User>(user!);
        return true;
    }

    public Task<User?> GetUserByAccountId(Guid accId) => _userRepository.GetUserByAccountId(accId);

    public async Task<ErrorOr<User?>> GetUserByEmail(string email)
    {
        User? user = await _userRepository.GetUserByEmail(email);

        if (user is null)
            return Errors.Errors.AuthEr.InvalidCredentials;

        return user;
    }

    public Task<User?> GetUserById(Guid id) => _userRepository.GetUserById(id);

    public Task<UserQueryModel> GetUserQuery(Guid userId) => _userRepository.GetUserQuery(userId);

    public async Task<ErrorOr<User?>> RegisterUser(string firstName, string lastName, string dateOfBirth, byte[] passwordHash, byte[] salt, string sex, string email)
    {
        User? user = await _userRepository.GetUserByEmail(email);

        if (user is not null) return Errors.Errors.AuthEr.InvalidCredentials;

        Role? role = await _userRepository.GetRoleByCode(RoleEnum.User.Code);

        Account acc = new()
        {
            Role = role!,
            Password = passwordHash,
            Salt = salt,
            Email = email
        };

        User newUser = new()
        {
            Surname = char.ToUpper(lastName[0]) + lastName[1..],
            Name = char.ToUpper(firstName[0]) + firstName[1..],
            Birthdate = BaseHelper.ConvertStringToDateOnly(dateOfBirth),
            Sex = sex[0],
            Account = acc
        };

        _userRepository.Save(newUser);

        return newUser;
    }

    public async Task<ErrorOr<bool>> RemoveCommerceFromFavorites(Guid userId, Guid commerceId)
    {
        User? user = await _userRepository.GetUserWithFavoriteCommerces(userId);

        if (user is null) return Errors.Errors.UserEr.UserNotFound;

        if (!user.FavoriteCommerces.Any(x => x.CommerceId == commerceId)) return Errors.Errors.UserEr.CommerceNotFound;

        FavoriteCommerce removeCommerce = user.FavoriteCommerces.Where(x => x.CommerceId == commerceId).FirstOrDefault()!;

        removeCommerce.IsDeleted = true;

        _userRepository.UpdateEntity<FavoriteCommerce>(removeCommerce);
        return true;
    }

    public async Task<ErrorOr<bool>> UpdateUser(Guid userId, string? firstName, string? lastName, string? dateOfBirth, SexEnum? sex)
    {
        User? user = await _userRepository.GetUserById(userId);

        if (user is null) return Errors.Errors.UserEr.UserNotFound;

        if (firstName is not null) user.Name = firstName;

        if (lastName is not null) user.Surname = lastName;

        if (sex is not null) user.Sex = sex.Code[0];

        if (dateOfBirth is not null)
        {
            DateOnly date = BaseHelper.ConvertStringToDateOnly(dateOfBirth);
            user.Birthdate = date;
        }

        _userRepository.UpdateEntity<User>(user!);
        return true;
    }
}

