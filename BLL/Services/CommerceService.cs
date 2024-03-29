﻿using BLL.Enums;
using BLL.IServices;
using DAL.IRepositories;
using ErrorOr;
using Microsoft.Extensions.Configuration;
using MODEL.Entities;
using MODEL.QueryModels.Commerce;

namespace BLL.Services;

public class CommerceService : ICommerceService
{
    private readonly ICommerceRepository _commerceRepository;
    private readonly IConfiguration _configuration;

    public CommerceService(ICommerceRepository commerceRepository, IConfiguration configuration)
    {
        _commerceRepository = commerceRepository;
        _configuration = configuration;
    }

    public async Task<ErrorOr<bool>> DeleteCommerce(Guid id)
    {
        Commerce? commerce = await _commerceRepository.GetCommerceById(id);

        if (commerce is null)
            return Errors.Errors.Commerce.CommerceNotFound;

        commerce.Account.IsDeleted = true;
        commerce.IsDeleted = true;

        _commerceRepository.UpdateEntity(commerce!);
        return true;
    }

    public Task<Commerce?> GetCommerceByAccountId(Guid accId) => _commerceRepository.GetCommerceByAccountId(accId);

    public Task<CommerceQueryModel?> GetCommerceQuery(Guid id) => _commerceRepository.GetCommerceQuery(id);

    public async Task<ErrorOr<Commerce?>> RegisterCommerce(string name, byte[] passwordHash, byte[] salt, Guid cityId, string email, string emailVericationCode)
    {
        Commerce? comm = await _commerceRepository.GetCommerceByEmail(email);

        if (comm is not null) return Errors.Errors.Auth.InvalidCredentials;

        Commerce? commByName = await _commerceRepository.GetCommerceByName(name);

        if (commByName is not null) return Errors.Errors.Commerce.CommerceAlreadyExist;

        City? city = await _commerceRepository.GetById<City>(cityId);

        if (city is null) return Errors.Errors.Commerce.CityNotFound;

        Role? role = await _commerceRepository.GetRoleByCode(RoleEnum.Commerce.Code);

        bool isEmailVerified = false;

        if (bool.Parse(_configuration["Email:EnableEmailVerification"]))
        {
            isEmailVerified = true;
        }

        Account acc = new()
        {
            Role = role!,
            Password = passwordHash,
            Salt = salt,
            Email = email,
            IsEmailVerified = isEmailVerified,
            VerifyEmailCode = emailVericationCode
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
        Commerce? commerce = await _commerceRepository.GetCommerceById(commerceId);

        if (commerce is null) return Errors.Errors.Commerce.CommerceNotFound;

        if (cityId is not null)
        {
            City? city = await _commerceRepository.GetById<City>(cityId.Value);
            if (city is null) return Errors.Errors.Commerce.CityNotFound;

            commerce.City = city;
        }

        if (name is not null) commerce.Name = name;

        if (logo is not null) commerce.Logo = logo;

        _commerceRepository.UpdateEntity(commerce!);
        return true;
    }
}

