﻿using API.Dtos;
using API.Models.CreateShop;
using API.Models.UpdateShop;
using BLL.IServices;
using ErrorOr;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using MODEL.Entities;

namespace API.Controllers;

public class ShopController : BaseController
{
    private readonly IShopService _shopService;

    public ShopController(IShopService shopService)
    {
        _shopService = shopService;
    }

    [HttpPost("shop")]
    public async Task<IActionResult> CreateShop(CreateShopModel req)
    {
        ValidationResult results = new CreateShopModelValidator().Validate(req);

        if (!results.IsValid) return ValidationBadRequestResponse(results);

        ErrorOr<Shop?> result = await _shopService.CreateShop(
            req.Name.Trim(),
            req.CommerceId,
            req.CityId,
            req.GeoPoint.Longitude,
            req.GeoPoint.Latitude,
            req.GeoPoint.Name.Trim(),
            req.GeoPoint.Address.Trim());

        return result.Match(
        authResult => Ok(new MessageDto("shop created.")),
        errors => Problem(errors));
    }

    [HttpPut("shop/{id}")]
    public async Task<IActionResult> UpdateShop(UpdateShopModel req)
    {
        ValidationResult results = new UpdateShopModelValidator().Validate(req);

        if (!results.IsValid) BadRequest(results.Errors.Select(x => x.ErrorMessage));

        //TODO: check if shop not belongs to commerce (token check)
        ErrorOr<Shop?> result = await _shopService.UpdateShop(
            req.CommerceId,
            req.Id,
            req?.Name?.Trim(),
            req?.CityId,
            req?.GeoPoint?.Longitude,
            req?.GeoPoint?.Latitude,
            req?.GeoPoint?.Name.Trim(),
            req?.GeoPoint?.Address);

        return result.Match(
        authResult => Ok(new MessageDto("shop updated.")),
        errors => Problem(errors));
    }
}
