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

    public async Task<ErrorOr<bool>> AddUserImpression(Guid id, string impression)
    {
        User? user = await _userRepository.GetUserWithImpressions(id);

        if (user is null) return Errors.Errors.User.UserNotFound;

        Impression newImpression = new()
        {
            Content = impression,
            Use = user
        };

        user.Impressions.Add(newImpression);

        _userRepository.UpdateEntity<User>(user);
        return true;
    }

    public async Task<ErrorOr<bool>> DeleteUser(Guid userId)
    {
        User? user = await _userRepository.GetUserById(userId);

        if (user is null)
            return Errors.Errors.User.UserNotFound;

        user.Acc.IsDeleted = true;
        user.IsDeleted = true;

        _userRepository.UpdateEntity<User>(user!);
        return true;
    }

    public async Task<ErrorOr<User?>> GetUserByEmail(string email)
    {
        User? user = await _userRepository.GetUserByEmail(email);

        if (user is null)
            return Errors.Errors.Auth.InvalidCredentials;

        return user;
    }

    public async Task<User?> GetUserById(Guid id) => await _userRepository.GetUserById(id);

    public Task<UserQueryModel> GetUserQuery(Guid userId) => _userRepository.GetUserQuery(userId);

    public async Task<ErrorOr<User?>> RegisterUser(string firstName, string lastName, string dateOfBirth, byte[] passwordHash, byte[] salt, string sex, string email)
    {
        User? user = await _userRepository.GetUserByEmail(email);

        if (user is not null) return Errors.Errors.Auth.InvalidCredentials;

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
            Surname = lastName,
            Name = firstName,
            Birthdate = BaseHelper.ConvertStringToDateOnly(dateOfBirth),
            Sex = sex[0],
            Acc = acc
        };

        _userRepository.Save(newUser);

        return newUser;
    }

    public async Task<ErrorOr<bool>> UpdateUser(Guid userId, string? firstName, string? lastName, string? dateOfBirth, SexEnum? sex)
    {
        User? user = await _userRepository.GetUserById(userId);

        if (user is null) return Errors.Errors.User.UserNotFound;

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

