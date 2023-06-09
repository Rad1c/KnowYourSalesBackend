﻿using ErrorOr;
using MODEL.Entities;

namespace BLL.IServices;

public interface IShopService
{
    public Task<Commerce?> GetCommerceById(Guid id);
    public Task<ErrorOr<Shop?>> CreateShop(string name, Guid commerceId, Guid cityId, decimal Longitude, decimal latitude, string? geoName, string? address);
    public Task<ErrorOr<Shop?>> UpdateShop(Guid commerceId, Guid id, string? name, Guid? cityId, decimal? Longitude, decimal? latitude, string? geoName, string? address);
}

