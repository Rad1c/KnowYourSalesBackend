using BLL.Enums;
using BLL.IServices;
using DAL.IRepositories;
using ErrorOr;
using MODEL.Entities;

namespace BLL.Services;

public class CommerceService : ICommerceService
{
    private readonly ICommerceRepository _commerceRepository;

    public CommerceService(ICommerceRepository commerceRepository)
    {
        _commerceRepository = commerceRepository;
    }

    public async Task<ErrorOr<Commerce?>> RegisterCommerce(string name, byte[] passwordHash, byte[] salt, Guid cityId, string email)
    {
        Commerce? comm = await _commerceRepository.GetCommerceByEmail(email);

        if (comm is not null) return Errors.Errors.Auth.InvalidCredentials;

        City? city = await _commerceRepository.GetById<City>(cityId);

        if (city is null) return Errors.Errors.Commerce.CityNotFound;

        Role? role = await _commerceRepository.GetRoleByCode(RoleEnum.Shop.Code);

        Account acc = new()
        {
            Role = role!,
            Password = passwordHash,
            Salt = salt,
            Email = email
        };

        Commerce newCommerce = new()
        {
            Name = name,
            Cit = city,
            Acc = acc
        };

        _commerceRepository.Save(newCommerce);

        return newCommerce;
    }
}

