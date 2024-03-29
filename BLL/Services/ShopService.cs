﻿using BLL.IServices;
using DAL.IRepositories;
using ErrorOr;
using MODEL.Entities;
using MODEL.QueryModels.Shop;

namespace BLL.Services;

public class ShopService : IShopService
{
    private readonly IShopRepository _shopRepository;

    public ShopService(IShopRepository shopRepository)
    {
        _shopRepository = shopRepository;
    }

    public async Task<ErrorOr<Shop?>> CreateShop(string name, Guid commerceId, Guid cityId, decimal Longitude, decimal latitude, string? address)
    {
        Commerce? commerce = await _shopRepository.GetCommerceWithShops(commerceId);

        if (commerce is null) return Errors.Errors.Shop.CommerceNotFound;

        if (commerce.Shops != null && commerce.Shops.Any(x => x.Name.ToLower().Equals(name)))
            return Errors.Errors.Shop.ShopAlreadyExist;

        City? city = await _shopRepository.GetById<City>(cityId);

        if (city is null) return Errors.Errors.Shop.CityNotFound;

        GeoPoint newGeo = new()
        {
            Latitude = latitude,
            Longitude = Longitude,
            Address = address
        };

        Shop newShop = new()
        {
            Name = name,
            City = city,
            Commerce = commerce,
            GeoPoint = newGeo
        };

        _shopRepository.Save(newShop);

        return newShop;
    }

    public Task<Commerce?> GetCommerceById(Guid id) => _shopRepository.GetCommerceById(id);

    public Task<ShopQueryModel?> GetShopQuery(Guid id) => _shopRepository.GetShopQuery(id);

    public async Task<ErrorOr<bool>> DeleteShop(Guid id)
    {
        Shop? shop = await _shopRepository.GetShop(id);

        if (shop is null)
            return Errors.Errors.Shop.ShopNotFound;

        shop.IsDeleted = true;

        _shopRepository.UpdateEntity(shop);
        return true;
    }

    public async Task<ErrorOr<Shop?>> UpdateShop(Guid commerceId, Guid id, string? name, Guid? cityId, decimal? Longitude, decimal? latitude, string? address)
    {
        Commerce? commerce = await _shopRepository.GetCommerceWithShops(commerceId);

        if (commerce is null) return Errors.Errors.Shop.CommerceNotFound;

        if (commerce.Shops == null || !commerce.Shops.Any(x => x.Id == id)) return Errors.Errors.Shop.ShopNotFound;

        Shop? shop = await _shopRepository
            .GetShop(commerce.Shops.Where(x => x.Id == id).FirstOrDefault().Id);

        if (name is not null) shop!.Name = name;

        if (cityId is not null)
        {
            City? city = await _shopRepository.GetById<City>(cityId.Value);
            if (city is null) return Errors.Errors.Shop.CityNotFound;

            shop!.City = city;
        }

        if (latitude is not null) shop!.GeoPoint.Latitude = latitude.Value;

        if (Longitude is not null) shop!.GeoPoint.Longitude = Longitude.Value;

        if (address is not null) shop!.GeoPoint.Address = address;

        _shopRepository.UpdateEntity<Shop>(shop);

        return shop;
    }
}

