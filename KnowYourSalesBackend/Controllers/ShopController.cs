using API.Dtos;
using API.Models;
using API.Models.Validators;
using BLL.IServices;
using BLL.Errors;
using ErrorOr;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using MODEL.Entities;
using MODEL.QueryModels.Shop;
using Microsoft.AspNetCore.Authorization;
using DAL.IRepositories;

namespace API.Controllers;

public class ShopController : BaseController
{
    private readonly IShopService _shopService;
    private readonly IShopRepository _shopRepository;
    private readonly ISessionService _sessionService;

    public ShopController(IShopService shopService, IShopRepository shopRepository, ISessionService sessionService)
    {
        _shopService = shopService;
        _shopRepository = shopRepository;
        _sessionService = sessionService;
    }

    // AllowAnonymous samo zbog testiranja -- 👍
    [AllowAnonymous]
    [HttpGet("shop/{id}")]
    public async Task<IActionResult> GetShop(Guid id)
    {
        ShopQueryModel? shop = await _shopService.GetShopQuery(id);

        if (shop is null) return Problem(Errors.Shop.ShopNotFound);

        return Ok(shop);
    }

    [AllowAnonymous]
    [HttpGet("shops/{id}")]
    public async Task<IActionResult> GetCommerceShops(Guid id)
    {
        return Ok(await _shopRepository.GetShopsQuery(id));
    }

    [HttpPost("shop")]
    public async Task<IActionResult> CreateShop(CreateShopModel req)
    {
        ValidationResult results = new CreateShopModelValidator().Validate(req);

        if (!results.IsValid) return ValidationBadRequestResponse(results);

        ErrorOr<Shop?> result = await _shopService.CreateShop(
            req.Name.Trim(),
            _sessionService.Id,
            req.CityId,
            req.GeoPoint.Longitude,
            req.GeoPoint.Latitude,
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

        //TODO: check if shop doesnt belong to commerce (token check)
        ErrorOr<Shop?> result = await _shopService.UpdateShop(
            _sessionService.Id,
            req.Id,
            req?.Name?.Trim(),
            req?.CityId,
            req?.GeoPoint?.Longitude,
            req?.GeoPoint?.Latitude,
            req?.GeoPoint?.Address);

        return result.Match(
        authResult => Ok(new MessageDto("shop updated.")),
        errors => Problem(errors));
    }
}
