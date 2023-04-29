using BLL.Enums;
using BLL.Helper;
using BLL.IServices;
using DAL.IRepositories;
using ErrorOr;
using MODEL.Entities;

namespace BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ErrorOr<User?>> RegisterUser(string firstName, string lastName, string dateOfBirth, byte[] passwordHash, byte[] salt, string sex, string email)
        {
            User? user = await _userRepository.GetUserByEmail(email);

            if (user is not null) return Errors.Errors.Auth.InvalidCredentials;

            Role? role = await _userRepository.GetRoleByCode(RoleEnum.User.Code);

            Account acc = new()
            {
                Rol = role!,
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
    }
}
