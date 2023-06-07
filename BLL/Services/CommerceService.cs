using BLL.Enums;
using BLL.IServices;
using DAL.IRepositories;
using ErrorOr;
using MODEL.Entities;
using MODEL.QueryModels.Commerce;

namespace BLL.Services;

public class CommerceService : ICommerceService
{
    private readonly ICommerceRepository _commerceRepository;

    public CommerceService(ICommerceRepository commerceRepository)
    {
        _commerceRepository = commerceRepository;
    }

    public async Task<ErrorOr<bool>> DeleteCommerce(Guid id)
    {
        MODEL.Entities.Commerce? commerce = await _commerceRepository.GetCommerceById(id);

        if (commerce is null)
            return Errors.Errors.CommerceEr.CommerceNotFound;

        commerce.Account.IsDeleted = true;
        commerce.IsDeleted = true;

        _commerceRepository.UpdateEntity<MODEL.Entities.Commerce>(commerce!);
        return true;
    }

    public Task<Commerce?> GetCommerceByAccountId(Guid accId) => _commerceRepository.GetCommerceByAccountId(accId);

    public Task<CommerceQueryModel?> GetCommerceQuery(Guid id) => _commerceRepository.GetCommerceQuery(id);

    public async Task<ErrorOr<MODEL.Entities.Commerce?>> RegisterCommerce(string name, byte[] passwordHash, byte[] salt, Guid cityId, string email)
    {
        MODEL.Entities.Commerce? comm = await _commerceRepository.GetCommerceByEmail(email);

        if (comm is not null) return Errors.Errors.AuthEr.InvalidCredentials;

        MODEL.Entities.Commerce? commByName = await _commerceRepository.GetCommerceByName(name);

        if (commByName is not null) return Errors.Errors.CommerceEr.CommerceAlreadyExist;

        City? city = await _commerceRepository.GetById<City>(cityId);

        if (city is null) return Errors.Errors.CommerceEr.CityNotFound;

        Role? role = await _commerceRepository.GetRoleByCode(RoleEnum.Shop.Code);

        Account acc = new()
        {
            Role = role!,
            Password = passwordHash,
            Salt = salt,
            Email = email
        };

        MODEL.Entities.Commerce newCommerce = new()
        {
            Name = name,
            City = city,
            Account = acc
        };

        _commerceRepository.Save(newCommerce);

        return newCommerce;
    }

    public async Task<ErrorOr<bool>> UpdateCommerce(Guid commerceId, string? name, string? logo, Guid? cityId)
    {
        MODEL.Entities.Commerce? commerce = await _commerceRepository.GetCommerceById(commerceId);

        if (commerce is null) return Errors.Errors.CommerceEr.CommerceNotFound;

        if (cityId is not null)
        {
            City? city = await _commerceRepository.GetById<City>(cityId.Value);
            if (city is null) return Errors.Errors.CommerceEr.CityNotFound;

            commerce.City = city;
        }

        if (name is not null) commerce.Name = name;

        if (logo is not null) commerce.Logo = logo;

        _commerceRepository.UpdateEntity<MODEL.Entities.Commerce>(commerce!);
        return true;
    }
}

